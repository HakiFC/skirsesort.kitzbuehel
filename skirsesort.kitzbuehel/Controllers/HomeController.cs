using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace skirsesort.kitzbuehel.Controllers
{
    public class HomeController : Controller
    {
        private int[][] matrix;
        private List<Tuple<int, int, string>> route;
        private List<Tuple<int, int, string>> routeTmp;

        public ActionResult Index()
        {
            return View();
        }

        private void CheckRight(Tuple<int, int, string> position)
        {
            try
            {
                int actual = matrix[position.Item1][position.Item2];
                int next = matrix[position.Item1][position.Item2 + 1];

                if (next < actual)
                {
                    int index = 0;
                    bool x = false;

                    while (index > -1)
                    {
                        int indexNext;
                        if (routeTmp.IndexOf(routeTmp.First(), index + 1) == -1)
                        {
                            indexNext = routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First());
                        }
                        else
                        {
                            indexNext = routeTmp.IndexOf(routeTmp.First(), index + 1) - index;
                        }

                        List<Tuple<int, int, string>> t = routeTmp.GetRange(index, indexNext).ToList();
                        List<Tuple<int, int, string>> t2 = routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).ToList();
                        t2.Add(new Tuple<int, int, string>(position.Item1, position.Item2 + 1, "R"));

                        index = routeTmp.IndexOf(routeTmp.First(), index + 1);

                        if (t.SequenceEqual(t2))
                        {
                            x = true;
                            break;
                        }
                    }

                    if (!x)
                    {
                        routeTmp.Add(new Tuple<int, int, string>(position.Item1, position.Item2 + 1, "R"));
                    }
                }
            }
            catch{ }
        }

        private void CheckDown(Tuple<int, int, string> position)
        {
            try
            {
                int actual = matrix[position.Item1][position.Item2];
                int next = matrix[position.Item1 + 1][position.Item2];

                if (next < actual)
                {
                    int index = 0;
                    bool x = false;

                    while (index > -1)
                    {
                        int indexNext;
                        if (routeTmp.IndexOf(routeTmp.First(), index + 1) == -1)
                        {
                            indexNext = routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First());
                        } else
                        {
                            indexNext = routeTmp.IndexOf(routeTmp.First(), index + 1) - index;
                        }

                        List<Tuple<int, int, string>> t = routeTmp.GetRange(index, indexNext).ToList();
                        List<Tuple<int, int, string>> t2 = routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).ToList();
                        t2.Add(new Tuple<int, int, string>(position.Item1 + 1, position.Item2, "D"));

                        index = routeTmp.IndexOf(routeTmp.First(), index + 1);

                        if (t.SequenceEqual(t2))
                        {
                            x = true;
                            break;
                        }
                    }

                    if (!x)
                    {
                        routeTmp.Add(new Tuple<int, int, string>(position.Item1 + 1, position.Item2, "D"));
                    }
                }
            }
            catch { }
        }

        private void CheckLeft(Tuple<int, int, string> position)
        {
            try
            {
                int actual = matrix[position.Item1][position.Item2];
                int next = matrix[position.Item1][position.Item2 - 1];

                if (next < actual)
                {
                    int index = 0;
                    bool x = false;

                    while (index > -1)
                    {
                        int indexNext;
                        if (routeTmp.IndexOf(routeTmp.First(), index + 1) == -1)
                        {
                            indexNext = routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First());
                        }
                        else
                        {
                            indexNext = routeTmp.IndexOf(routeTmp.First(), index + 1) - index;
                        }

                        List<Tuple<int, int, string>> t = routeTmp.GetRange(index, indexNext).ToList();
                        List<Tuple<int, int, string>> t2 = routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).ToList();
                        t2.Add(new Tuple<int, int, string>(position.Item1, position.Item2 - 1, "L"));

                        index = routeTmp.IndexOf(routeTmp.First(), index + 1);

                        if (t.SequenceEqual(t2))
                        {
                            x = true;
                            break;
                        }
                    }

                    if (!x)
                    {
                        routeTmp.Add(new Tuple<int, int, string>(position.Item1, position.Item2 - 1, "L"));
                    }
                }
            }
            catch { }
        }

        private void CheckUp(Tuple<int, int, string> position)
        {
            try
            {
                int actual = matrix[position.Item1][position.Item2];
                int next = matrix[position.Item1 - 1][position.Item2];

                if (next < actual)
                {
                    int index = 0;
                    bool x = false;

                    while (index > -1)
                    {
                        int indexNext;
                        if (routeTmp.IndexOf(routeTmp.First(), index + 1) == -1)
                        {
                            indexNext = routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First());
                        }
                        else
                        {
                            indexNext = routeTmp.IndexOf(routeTmp.First(), index + 1) - index;
                        }

                        List<Tuple<int, int, string>> t = routeTmp.GetRange(index, indexNext).ToList();
                        List<Tuple<int, int, string>> t2 = routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).ToList();
                        t2.Add(new Tuple<int, int, string>(position.Item1 - 1, position.Item2, "U"));

                        index = routeTmp.IndexOf(routeTmp.First(), index + 1);

                        if (t.SequenceEqual(t2))
                        {
                            x = true;
                            break;
                        }
                    }

                    if (!x)
                    {
                        routeTmp.Add(new Tuple<int, int, string>(position.Item1 - 1, position.Item2, "U"));
                    }
                }
            }
            catch { }
        }

        public JsonResult CalculateRoutes()
        {
            bool r = false;
            var file = Request.Files["file"];

            if (file != null)
            {
                try
                {
                    string name = "tmp_" + new FileInfo(file.FileName);
                    string path = System.Web.HttpContext.Current.Server.MapPath("~/" + name);

                    file.SaveAs(path);

                    var temp = System.IO.File.ReadAllLines(path);

                    int a = Int32.Parse(temp[0].Split(' ')[0]);
                    int b = Int32.Parse(temp[0].Split(' ')[1]);

                    matrix = new int[a][];
                    route = new List<Tuple<int, int, string>>();

                    for (var i = 0; i < a; i++)
                    {
                        matrix[i] = temp[i + 1].Split(' ').Select(Int32.Parse).ToArray();
                    }

                    for(var i = 0; i < matrix.Length; i++)
                    {
                        for (var k = 0; k < b; k++)
                        {
                            int ini = 0;
                            routeTmp = new List<Tuple<int, int, string>>();
                            routeTmp.Add(new Tuple<int, int, string>(i, k, "I"));
                            if ((k == 0 && matrix[i][k] >= matrix[i][k + 1] && matrix[i][k] >= matrix[i + 1][k]) ||
                                (k == (b - 1) && matrix[i][k] >= matrix[i][k - 1] && matrix[i][k] >= matrix[i + 1][k]) ||
                                (k != 0 && k != (b - 1) && matrix[i][k] >= matrix[i][k + 1] && matrix[i][k] >= matrix[i + 1][k] && matrix[i][k] >= matrix[i][k - 1]))
                            {
                                bool run = true;

                                while (run)
                                {
                                    int n = routeTmp.Count;

                                    CheckRight(routeTmp.Last());
                                    if (routeTmp.Count == n)
                                    {
                                        CheckDown(routeTmp.Last());
                                        if (routeTmp.Count == n)
                                        {
                                            CheckLeft(routeTmp.Last());
                                            if (routeTmp.Count == n)
                                            {
                                                CheckUp(routeTmp.Last());
                                                if (routeTmp.Count == n)
                                                {
                                                    if (routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).Count >= route.Count)
                                                    {
                                                        if (routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).Count == route.Count)
                                                        {
                                                            int x = matrix[route.First().Item1][route.First().Item2] - matrix[route.Last().Item1][route.Last().Item2];
                                                            int y = matrix[routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).First().Item1][routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).First().Item2] - matrix[routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).Last().Item1][routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())).Last().Item2];
                                                        
                                                            if (y > x)
                                                            {
                                                                route.Clear();
                                                                route.AddRange(routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            route.Clear();
                                                            route.AddRange(routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First())));
                                                        }
                                                    }

                                                    ini = routeTmp.Count;
                                                    routeTmp.AddRange(routeTmp.GetRange(routeTmp.LastIndexOf(routeTmp.First()), routeTmp.Count - routeTmp.LastIndexOf(routeTmp.First()) - 1));
                                                    if (routeTmp.Count == ini)
                                                    {
                                                        run = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            string res = "";

            for(var i = 0; i < route.Count; i++)
            {
                res += matrix[route.ElementAt(i).Item1][route.ElementAt(i).Item2].ToString() + " - ";
            }


            return Json(new { Route = res });
        }
    }
}