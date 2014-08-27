using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InputSite.Interfaces;
using InputSite.Model;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Nancy.ViewEngines;
using Version = Lucene.Net.Util.Version;

namespace InputSite.Services
{
    public class ArticleStorage : IArticleStorage
    {
        private readonly IArticleLocator _articleLocator;

        private static bool _alreadyLoaded;
        private static RAMDirectory _luceneStorage; // no need for this to be static, but until i am sure it is :-)
        private static IndexWriter _indexWriter;
        private static Analyzer _standardAnalyzer;

        public ArticleStorage(IArticleLocator articleLocator)
        {
            _articleLocator = articleLocator;
            InitializeStorage();
        }

        private void InitializeStorage()
        {
            EnsureStorage();

            var articles = _articleLocator.Articles();
            foreach (var article in articles)
            {
                using (var sr = new StreamReader(article))
                {
                    var markdown = sr.ReadToEnd();
                    var resource = _articleLocator.PathToResource(article);

                    Map(new ArticleParser(markdown, resource));
                }
            }

            _alreadyLoaded = true;
        }

        private static void EnsureStorage()
        {
            if (_alreadyLoaded) return;

            _standardAnalyzer = new StandardAnalyzer(Version.LUCENE_30);
            _luceneStorage = new RAMDirectory();
            _indexWriter = new IndexWriter(_luceneStorage, new StandardAnalyzer(Version.LUCENE_30), true, IndexWriter.MaxFieldLength.LIMITED);
        }

        private static void Map(ArticleParser parser)
        {
            var doc = new Document();

            if (!string.IsNullOrEmpty(parser.Category))
                doc.Add(new Field("category", parser.Category, Field.Store.YES, Field.Index.ANALYZED));

            doc.Add(new Field("author", parser.Author, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("title", parser.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("body", parser.Body, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("abstract", parser.Abstract, Field.Store.YES, Field.Index.ANALYZED));

            foreach (var tag in parser.Tags)
            {
                doc.Add(new Field("tags", tag, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));    
            }

            // TODO : Should probably be stored separatly, in a profile index instead.
            foreach (var role in parser.Roles)
            {
                doc.Add(new Field("roles", role, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));    
            }

            var parts = parser.ResourceName.Split(new[]{'/'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                doc.Add(new Field("resource", part, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            }

            doc.Add(new Field("date", DateTools.DateToString(parser.BlogDate, DateTools.Resolution.DAY), Field.Store.YES, Field.Index.ANALYZED));
            
            _indexWriter.AddDocument(doc);
            _indexWriter.Optimize();
        }

        public IEnumerable<ArticleModel> ArticlesByFreeText(string search)
        {
            var reader = _indexWriter.GetReader();
            using (var searcher = new IndexSearcher(reader))
            {
                var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "body", "title", "tags", "roles" }, _standardAnalyzer);
                var query = parser.Parse(search);

                var hits = searcher.Search(query, 100);
                return BuildResult(hits, searcher);
            }
        }

        public IEnumerable<ArticleModel> ArticlesByCategory(string category)
        {
            var reader = _indexWriter.GetReader();
            using (var searcher = new IndexSearcher(reader))
            {
                var parser = new QueryParser(Version.LUCENE_30, "resource", _standardAnalyzer);

                var parts = category.Split('/').Select(s => string.Concat((string) "+",(string) s)).ToArray();
                var baked = string.Join(" ", parts);

                var query = parser.Parse(baked);
                var hits = searcher.Search(query, 100);

                return BuildResult(hits, searcher);
            } 
        }


        public IEnumerable<ArticleModel> ArticlesByTags(IEnumerable<string> tags)
        {
            var reader = _indexWriter.GetReader();
            using (var searcher = new IndexSearcher(reader))
            {
//                var parser = new QueryParser(Version.LUCENE_30, "tags", _standardAnalyzer);
                var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { "tags", "roles" }, _standardAnalyzer);                

                var baked = string.Join(" or ", tags);
                var query = parser.Parse(baked);

                var hits = searcher.Search(query, 10);

                return BuildResult(hits, searcher);
            }                        
        }

        public IEnumerable<ArticleModel> ArticlesByAuthor(string author)
        {
            var reader = _indexWriter.GetReader();
            using (var searcher = new IndexSearcher(reader))
            {
                var parser = new QueryParser(Version.LUCENE_30, "author", _standardAnalyzer);

                var query = parser.Parse(author);
                var hits = searcher.Search(query, 100);

                return BuildResult(hits, searcher);
            }
        }

        public IEnumerable<string> TagCloud()
        {
            // The good thing is that this was really easy, the bad thing is that tags on articles
            // that are not yet posted shows up. Need to deal with that little detail later.

            var reader = _indexWriter.GetReader();
            var terms = reader.Terms(new Term("tags", ""));

            while (terms.Term.Field.Equals("tags"))
            {
                yield return terms.Term.Text;
                if (!terms.Next()) break;
            }
        } 

        private static IEnumerable<ArticleModel> BuildResult(TopDocs hits, IndexSearcher searcher)
        {
            for (var ii = 0; ii < hits.TotalHits; ii++)
            {
                var aDock = searcher.Doc(hits.ScoreDocs[ii].Doc);

                var tags = aDock.GetValues("tags");
                var roles = aDock.GetValues("roles");
                var date = DateTools.StringToDate(aDock.Get("date"));
                var resource = string.Join("/", aDock.GetValues("resource"));

                yield return
                    new ArticleModel(aDock.Get("category"), aDock.Get("author"), aDock.Get("title"), aDock.Get("abstract"), tags, roles, resource, date);
            }
        }

    }
}