using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSALConnect.Services
{
    public static class GraphService 
    {
        public static IGraphService Instance { get; } = new GraphServiceGraph();
    }
}