﻿using System.Text.RegularExpressions;
using System.Xml.Schema;
using NUnit.Framework;

namespace DijkstrasAlgorithm.Tests
{
    [TestFixture]
    public class MinPathTest
    {
        [Test]
        public void NoGraph_NoPathZeroLength()
        {
            AssertMinPath("", 0, "[]");     // empty graph
            AssertMinPath("A", 0, "[]");    // one node
            AssertMinPath("BC1", 0, "[]");  // no start or end
            AssertMinPath("AC1", 0, "[]");  // no end
            AssertMinPath("BZ1", 0, "[]");  // no start
        }

        [Test]
        public void OneEdge()
        {
            AssertMinPath("AZ1", 1, "[A, Z]");
            AssertMinPath("AZ2", 2, "[A, Z]");
        }

        [Test]
        public void TwoEdges()
        {
            AssertMinPath("AB1,BZ1", 2, "[A, B, Z]");
            AssertMinPath("BZ1,AB1", 2, "[A, B, Z]");
            AssertMinPath("AX1,YZ1", 0, "[]");
        }

        [Test]
        public void ThreeEdges()
        {
            AssertMinPath("AB2,BC3,CZ4", 9, "[A, B, C, Z]");
            AssertMinPath("BC3,CZ4,AB2", 9, "[A, B, C, Z]");
        }

        [Test]
        public void OnlyOnePath()
        {
            AssertMinPath("AB1,BC2,CZ3,BD4,DE6", 6, "[A, B, C, Z]");
            AssertMinPath("AB1,BC2,CD3,CZ3", 6, "[A, B, C, Z]");
        }

        [Test]
        public void ParallelPaths()
        {
            AssertMinPath("AB1,BZ2,AZ1", 1, "[A, Z]");
        }

        private void AssertMinPath(string graph, int length, string path)
        {
            var pf = MakePathFinder(graph);
            if (length > 0)
                Assert.That(pf.GetLength(), Is.EqualTo(length));
            if (!string.IsNullOrEmpty(path))
                Assert.That(pf.GetPath(), Is.EqualTo(path));
        }

        private PathFinder MakePathFinder(string graph)
        {
            var pf = new PathFinder();
            var edgePattern = new Regex("(\\D+)(\\D+)(\\d+)");
            var edges = graph.Split(',');
            foreach (var edge in edges)
            {
                if (edgePattern.IsMatch(edge))
                {
                    var begin = edgePattern.Replace(edge, "$1");
                    var end = edgePattern.Replace(edge, "$2");
                    var length = int.Parse(edgePattern.Replace(edge, "$3"));
                    pf.AddEdge(begin, end, length);
                }
            }
            pf.FindPath("A", "Z");
            return pf;
        }
    }
}
