﻿using System.IO;

namespace InputSite.Bootstrap
{
    public class EnsureThatRouteContainDateParts : IRouteEvaluator
    {
        public bool Ensure(FileInfo fileInfo)
        {
            int a;
            return int.TryParse(fileInfo.Directory.Name, out a);
        }
    }
}