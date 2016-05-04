using Microsoft.WindowsAzure;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Input.Site.WebJob
{


    public class GitClient 
    {
        private static class CreateTreeMode
        {
            public const string blob = "100644";
            public const string executable = "100755";
            public const string subdirectoryortree = "040000";
            public const string submoduleorcommit = "16000";
            public const string blobspecifyingpathofsymlink = "120000";
        }
        private enum CreateTreeType { blob, tree, commit };
        private readonly string baseuri, branch, owner, repo, fileprefix, blogpath, gittoken;
        private string headsha, headurl, commitsha, treesha, treeurl, fileblobsha, newtreesha, newcommitsha;
        private HttpClient client;

        public GitClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "custom");
            gittoken = CloudConfigurationManager.GetSetting("git-token");
            client.DefaultRequestHeaders.Add("Authorization", "token " + gittoken);
            branch = CloudConfigurationManager.GetSetting("git-branch");
            owner = CloudConfigurationManager.GetSetting("git-owner");
            repo = CloudConfigurationManager.GetSetting("git-repo");
            baseuri = CloudConfigurationManager.GetSetting("git-baseuri");
            fileprefix = CloudConfigurationManager.GetSetting("git-fileprefix");
            blogpath = CloudConfigurationManager.GetSetting("git-blogpath");                      
        }

        public async Task<string> GetLastBlogPostTimeStamp()
        {
            // The latest slack timestamp can be found in the blog folder, in the filename of the blogposts
            
            long lastcommitedblogpost = 1000000000000001;       // sometime 2001, fallback if no posts...
            Uri reqUri = new Uri($"{baseuri}/repos/{owner}/{repo}/contents/{ (blogpath == "" ? "" : blogpath) }?ref={branch}");
            HttpResponseMessage response = await client.GetAsync(reqUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JArray jarr = JArray.Parse(jsonstring);
                foreach (JObject Jobj in jarr.Children())
                {
                    string filename = Jobj.Property("name").Value.ToString();
                    if (filename.StartsWith(fileprefix))
                    {
                        string sts = filename.Substring(8, 16);
                        long lts = Convert.ToInt64(sts);
                        if (lts > lastcommitedblogpost)
                            lastcommitedblogpost = lts;
                    }
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Could not authorize request to git. Wrong Token or repo?");
            }
            string lastcommitedblogpoststring = lastcommitedblogpost.ToString().Insert(10, ".");
            return lastcommitedblogpoststring;
        }

        // Inspired by this blogpost http://www.levibotelho.com/development/commit-a-file-with-the-github-api
        public async Task CommitFile(Message message)
        {
            string filename = CloudConfigurationManager.GetSetting("git-filenamestructure").ToString().Replace("[ts]", message.ts.Replace(".", ""));
            Console.WriteLine($"Commiting file {filename} ");
            await GetReferenceToHead();
            Console.WriteLine($"Got reference to head, {headurl}");
            await GrabCommitThatHeadPointsTo();
            Console.WriteLine($"Got reference to head, {commitsha} and tree url {treeurl}");
            await PostBlob(message);
            Console.WriteLine($"Posted File Blob {fileblobsha}");
            //await GetTreeThatCommitPointsTo();    //we alreade have this sha
            await CreateTreeContainingNewFile(filename);
            Console.WriteLine($"Created the new tree {newtreesha}");
            await CreateNewCommit(CloudConfigurationManager.GetSetting("git-commitmessage").ToString().Replace("[ts]", message.ts.Replace(".", "")));
            Console.WriteLine($"Created new commit, {newcommitsha}");
            await UpdateHead();
            Console.WriteLine($"Moved head...");
        }
        // Inspired by this blogpost http://www.levibotelho.com/development/commit-a-file-with-the-github-api
        public async Task CommitFile(File file)
        {
            string filename = CloudConfigurationManager.GetSetting("git-filenamestructure").ToString().Replace("[ts]", file.timestamp.ToString() + ".999999" );
            Console.WriteLine($"Commiting file {filename} ");
            await GetReferenceToHead();
            Console.WriteLine($"Got reference to head, {headurl}");
            await GrabCommitThatHeadPointsTo();
            Console.WriteLine($"Got reference to head, {commitsha} and tree url {treeurl}");
            await PostBlob(file);
            Console.WriteLine($"Posted File Blob {fileblobsha}");
            //await GetTreeThatCommitPointsTo();    //we alreade have this sha
            await CreateTreeContainingNewFile(filename);
            Console.WriteLine($"Created the new tree {newtreesha}");
            await CreateNewCommit(CloudConfigurationManager.GetSetting("git-commitmessage").ToString().Replace("[ts]", file.timestamp.ToString() + ".999999"));
            Console.WriteLine($"Created new commit, {newcommitsha}");
            await UpdateHead();
            Console.WriteLine($"Moved head...");
        }

        private async Task GetReferenceToHead()
        {
            Uri reqUri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/refs/heads/{branch}");
            HttpResponseMessage response = await client.GetAsync(reqUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj = JObject.Parse(jsonstring);
                headsha = (string)jobj["object"]["sha"];
                headurl = (string)jobj["object"]["url"];
            }
        }
        private async Task GrabCommitThatHeadPointsTo()
        {
            Uri reqUri = new Uri(headurl);
            HttpResponseMessage response = await client.GetAsync(reqUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj = JObject.Parse(jsonstring);
                commitsha = (string)jobj["sha"];
                treesha = (string)jobj["tree"]["sha"];
                treeurl = (string)jobj["tree"]["url"];
            }
        }

        private async Task PostBlob(Message message)
        {
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/blobs");
            JObject jobj = JObject.FromObject(new
            {
                content = FormatBlogPostMarkdown(message),
                Encoding =   "utf-8"            // "base64"
            });
            string stri = jobj.ToString();
            var stringContent = new StringContent(jobj.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);
                fileblobsha = (string)jobj2["sha"];
            }
        }

        private async Task PostBlob(File file)
        {
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/blobs");
            //string posttext = GetPostFileContent(file.url_private_download);
            JObject jobj = JObject.FromObject(new
            {
                content = FormatBlogPostMarkdown(file, file.preview),
                Encoding = "utf-8"            // "base64"
            });
            string stri = jobj.ToString();
            var stringContent = new StringContent(jobj.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);
                fileblobsha = (string)jobj2["sha"];
            }
        }

        private async Task GetTreeThatCommitPointsTo()
        {
            Uri reqUri = new Uri(treeurl);
            HttpResponseMessage response = await client.GetAsync(reqUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj = JObject.Parse(jsonstring);
                treesha = (string)jobj["sha"];                
            }
        }

        private async Task CreateTreeContainingNewFile(string filename)
        {   
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/trees");
            Uri geturi = new Uri($"{baseuri}/repos/{owner}/{repo}/git/trees/{treesha}?recursive=1");
            JObject jobjexistingtree;
            JToken jtree = JObject.FromObject(new
            {
                path = blogpath + filename,
                mode = CreateTreeMode.blob,
                type = Enum.GetName(typeof(CreateTreeType), CreateTreeType.blob),
                sha = fileblobsha
            });            
            HttpResponseMessage treeresponse = await client.GetAsync(geturi);
            if (treeresponse.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Could not read blog tree");
            string jsonstring = await treeresponse.Content.ReadAsStringAsync();
            jobjexistingtree = JObject.Parse(jsonstring);
            JArray trimmedtree = TrimTree(jobjexistingtree["tree"] as JArray);
            trimmedtree.Add(jtree);
            jobjexistingtree["tree"] = trimmedtree;   // apparently this is needed ?!?

            string stri = jobjexistingtree.ToString();
            var stringContent = new StringContent(jobjexistingtree.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);
                newtreesha = (string)jobj2["sha"];
            }
        }

        private JArray TrimTree(JArray tree)
        {
            foreach (JObject Jobj in tree)
            {
                Jobj.Property("url").Remove();
            }
            return tree;
        }

        private async Task CreateNewCommit(string messagestring)
        {
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/commits");
            
            JObject jobj = JObject.FromObject(new
            {
                message = messagestring,
                parents = new string[] { commitsha } ,
                tree = newtreesha
            });
            string stri = jobj.ToString();
            var stringContent = new StringContent(jobj.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);
                newcommitsha = (string)jobj2["sha"];
            }
        }

        private async Task UpdateHead()
        {
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/refs/heads/{branch}");

            JObject jobj = JObject.FromObject(new
            {
                sha = newcommitsha
            });
            string stri = jobj.ToString();
            var stringContent = new StringContent(jobj.ToString());            
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, uri)
            {
                Content = stringContent
            };
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);                
            }
        }

        private string FormatBlogPostMarkdown(Message message)
        {            
            return FormatBlogPostMarkdown(message.user, SlackHelper.SlackTimeStampToDateTime(message.ts).ToString(), message.text);
        }

        private string FormatBlogPostMarkdown(File file, string content)
        {                       
            return FormatBlogPostMarkdown(file.username, SlackHelper.UnixTimeStampToDateTime(file.timestamp).ToString(), content);
        }

        private string FormatBlogPostMarkdown(string author, string datestring, string content)
        {
            string str = $@"@Master['_layout/article-page.html']

@Meta Author : {author}
Title: Blog Post Date: {datestring} Tags: Blog 
@EndMeta

@Section['Content']

{content}

@EndSection";
            return str;
        }

        private string GetPostFileContent(string url)
        {
            return "här ska du skriva något som hämtar från urlen...";
        }

        // The easy way replacing tree in git
        private async Task CreateTreeContainingNewFileEasyDeletingOldFiles(string filename)
        {
            Uri uri = new Uri($"{baseuri}/repos/{owner}/{repo}/git/trees");

            JObject jtree = JObject.FromObject(new
            {
                path = filename,
                mode = CreateTreeMode.blob,
                type = Enum.GetName(typeof(CreateTreeType), CreateTreeType.blob),
                sha = fileblobsha
            });
            JObject jobj = JObject.FromObject(new
            {
                basetree = treesha,
                tree = new[] { jtree }
            });
            string stri = jobj.ToString();
            var stringContent = new StringContent(jobj.ToString());

            HttpResponseMessage response = await client.PostAsync(uri, stringContent);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                string jsonstring = await response.Content.ReadAsStringAsync();
                JObject jobj2 = JObject.Parse(jsonstring);
                newtreesha = (string)jobj2["sha"];
            }
        }
        private async Task<string> GetBranchSha(string branchname)
        {
            Uri reqUri = new Uri($"{baseuri}/repos/{owner}/{repo}/branches");

            HttpResponseMessage response = await client.GetAsync(reqUri);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                string branchesJson = await response.Content.ReadAsStringAsync();
                JArray stuff = JArray.Parse(branchesJson);
                string sha = (from br in stuff
                              where (string)br["name"] == branch
                              select (string)br["commit"]["sha"]).FirstOrDefault();
                return sha;
            }
            return null;
        }
    }
}