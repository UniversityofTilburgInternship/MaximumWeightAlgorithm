using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class MaxWeightMatching
    {
        private static MyList<Edge> _myEdges = new MyList<Edge>();
        private static int _amountOfEdges;
        private static int _amountOfNodes;
        private static int _maxWeight;
        private static List<int> _endpoint = new List<int>();
        private static List<List<int>> _neightbend = new List<List<int>>();
        private static MyList<int> _mate = new MyList<int>();
        private static MyList<int> _label = new MyList<int>();
        private static MyList<int> _labelend = new MyList<int>();
        private static MyList<int> _inblossom = new MyList<int>();
        private static MyList<int> _blossomparent = new MyList<int>();
        private static MyList<MyList<int>> _blossomchilds = new MyList<MyList<int>>();
        private static MyList<int> _blossombase = new MyList<int>();
        private static MyList<MyList<int>> _blossomdps = new MyList<MyList<int>>();
        private static MyList<int> _bestedge = new MyList<int>();
        private static MyList<MyList<int>> _blossombestedges = new MyList<MyList<int>>();
        private static MyList<int> _unusedblossoms = new MyList<int>();
        private static MyList<int> _dualvar = new MyList<int>();
        private static MyList<bool> _allowedge = new MyList<bool>();
        private static MyList<int> _queue = new MyList<int>();
        private static bool CHECK_DELTA = false;
        private static bool CHECK_OPTIMUM = true;

        public static List<int> MaxWMatching(List<Edge> edges)
        {
            // if edges is empty
            if (edges.Count <= 0)
                return _mate.ToList();

            _myEdges = new MyList<Edge>(edges);

            // Count nodes
            _amountOfEdges = edges.Count;
            _amountOfNodes = 0;

            foreach (var edge in edges)
            {
                if (!(edge[0] >= 0 && edge[1] >= 0 && edge[0] != edge[1]))
                    throw new Exception("Assert Error");
                if (edge[0] >= _amountOfNodes) _amountOfNodes = edge[0] + 1;
                if (edge[1] >= _amountOfNodes) _amountOfNodes = edge[1] + 1;
            }
            Console.WriteLine("Applying Edmonds Algorithm with {0} nodes transformed by Siloach reducer\n",
                _amountOfNodes);

            for (var i = 0; i < _amountOfEdges; i++)
            {
                _maxWeight = Math.Max(0, Math.Max(_maxWeight, edges[i][2]));
            }

            for (var i = 0; i < 2 * _amountOfEdges; i++)
            {
                _endpoint.Add(edges[DivideAndFloor(i, 2)][i % 2]);
            }

            for (var i = 0; i < _amountOfNodes; i++)
            {
                _neightbend.Add(new List<int>());
            }

            for (var k = 0; k < _amountOfEdges; k++)
            {
                var i = edges[k][0];
                var j = edges[k][1];

                _neightbend[i].Add(2 * k + 1);
                _neightbend[j].Add(2 * k);
            }

            for (var i = 0; i < _amountOfNodes; i++)
            {
                _mate.Add(-1);
                _inblossom.Add(i);
                _blossombase.Add(i);
                _bestedge.Add(-1);
                _blossomdps.Add(new MyList<int>());
                _dualvar.Add(_maxWeight);
            }

            for (var i = 0; i < 2 * _amountOfNodes; i++)
            {
                _label.Add(0);
                _labelend.Add(-1);
                _blossomparent.Add(-1);
                _blossomchilds.Add(new MyList<int>());
                _blossombestedges.Add(new MyList<int>());
            }
            for (var i = 0; i < _amountOfNodes; i++)
            {
                _blossombase.Add(-1);
                _dualvar.Add(0);
            }
            for (var i = _amountOfNodes; i < 2 * _amountOfNodes; i++)
            {
                _unusedblossoms.Add(i);
            }

            for (var i = 0; i < _amountOfEdges; i++)
            {
                _allowedge.Add(false);
            }


            for (var t = 0; t < _amountOfEdges; t++)
            {
                var label1 = new MyList<int>(2 * _amountOfNodes);
                for (var i = 0; i < label1.Count; i++)
                {
                    label1[i] = 0;
                }
                _label = label1;

                var label2 = new MyList<int>(2 * _amountOfNodes);
                for (var i = 0; i < label2.Count; i++)
                {
                    label2[i] = -1;
                }
                _bestedge = label2;

                for (var i = _amountOfNodes; i < 2 * _amountOfNodes; i++)
                {
                    _blossombestedges[i] = new MyList<int>();
                }
                for (var i = 0; i < _amountOfEdges; i++)
                {
                    _allowedge[i] = false;
                }

                _queue = new MyList<int>();

                for (var n = 0; n < _amountOfNodes; n++)
                {
                    if (_mate[n] == -1 && _label[_inblossom[n]] == 0) AssignLabel(n, 1, -1);
                }
Console.WriteLine("queue size: " + _queue.Size() + "\n " + _queue.ToString());
                var augmented = false;
                while (true)
                {
                    while (_queue.Size() > 0 && !augmented)
                    {
                        var n = _queue[_queue.Size() - 1];
                        Console.WriteLine(n);
                        _queue.RemoveLast();

                        if (_label[_inblossom[n]] != 1) throw new Exception("Assert Error");

                        for (var i = 0; i < _neightbend[n].Count; i++)
                        {
                            var p = _neightbend[n][i];
                            var k = DivideAndFloor(p, 2);
                            var w = _endpoint[p];
                            var kslack = 0;

                            if (_inblossom[n] == _inblossom[w])
                                continue;
                            if (!_allowedge[k])
                            {
                                kslack = Slack(k);
                                if (kslack <= 0)
                                    _allowedge[k] = true;
                            }
                            if (_allowedge[k])
                            {
                                if (_label[_inblossom[w]] == 0)
                                    AssignLabel(w, 2, p ^ 1);
                                else if (_label[_inblossom[w]] == 1)
                                {
                                    var Base = ScanBlossom(n, w);
                                    if (Base >= 0)
                                        AddBlossom(Base, k);
                                    else
                                    {
                                        AugmentMatching(k);
                                        augmented = true;
                                        break;
                                    }
                                }
                                else if (_label[w] == 0)
                                {
                                    if (_label[_inblossom[w]] != 2) throw new Exception(" Assert error");
                                    _label[w] = 2;
                                    _labelend[w] = p ^ 1;
                                }
                            }
                            else if (_label[_inblossom[w]] == 1)
                            {
                                var b = _inblossom[n];
                                if (_bestedge[b] == -1 || kslack < Slack(_bestedge[b]))
                                    _bestedge[b] = k;
                            }
                            else if (_label[w] == 0)
                            {
                                if (_bestedge[w] == -1 || kslack < Slack(_bestedge[w]))
                                    _bestedge[w] = k;
                            }
                        }
                    }
                    if (augmented)
                        break;
                    var deltatype = -1;
                    int deltaEdge = 0, deltaBlossom = 0;

                    //TODO CHEKDATA

                    deltatype = 1;
                    var min = _dualvar[0];
                    for (var i = 0; i < _amountOfNodes; i++)
                    {
                        min = Math.Min(min, _dualvar[i]);
                    }
                    var delta = min;
                    for (var n = 0; n < _amountOfNodes; n++)
                    {
                        if (_label[_inblossom[n]] == 0 && _bestedge[n] != -1)
                        {
                            var d = Slack(_bestedge[n]);
                            if (deltatype == -1 || d < delta)
                            {
                                delta = d;
                                deltatype = 2;
                                deltaEdge = _bestedge[n];
                            }
                        }
                    }
                    for (var b = 0; b < 2 * _amountOfNodes; b++)
                    {
                        if (_blossomparent[b] == -1 && _label[b] == 1 && _bestedge[b] != -1)
                        {
                            var kslack = Slack(_bestedge[b]);
                            var d = DivideAndFloor(kslack, 2);
                            if (deltatype == -1 || d < delta)
                            {
                                delta = d;
                                deltatype = 3;
                                deltaEdge = _bestedge[b];
                            }
                        }
                    }
                    for (var b = 0; b < _amountOfNodes; b++)
                    {
                        if (_blossombase[b] >= 0 && _blossomparent[b] == -1 && _label[b] == 2 &&
                            (deltatype == -1 || _dualvar[b] < delta))
                        {
                            delta = _dualvar[b];
                            deltatype = 4;
                            deltaBlossom = b;
                        }
                    }
                    if (deltatype == -1)
                    {
                        deltatype = 1;
                        min = _dualvar[0];
                        for (var i = 0; i < _amountOfNodes; i++)
                        {
                            min = Math.Min(min, _dualvar[i]);
                        }
                        delta = Math.Max(min, 0);
                    }
                    for (var n = 0; n < _amountOfNodes; n++)
                    {
                        if (_label[_inblossom[n]] == 1)
                            _dualvar[n] -= delta;
                        else if (_label[_inblossom[n]] == 2)
                            _dualvar[n] += delta;
                    }
                    for (var b = _amountOfNodes; b < 2 * _amountOfNodes; b++)
                    {
                        if (_blossombase[b] >= 0 && _blossomparent[b] == -1)
                        {
                            if (_label[b] == 1)
                                _dualvar[b] += delta;
                            else if (_label[b] == 2)
                                _dualvar[b] -= delta;
                        }
                    }
                    if (deltatype == 1)
                        break;
                    if (deltatype == 2)
                    {
                        _allowedge[deltaEdge] = true;
                        var i = _myEdges[deltaEdge][0];
                        var j = _myEdges[deltaEdge][1];
                        var wt = _myEdges[deltaEdge][2];

                        if (_label[_inblossom[i]] == 0)
                        {
                            var tmp = i;
                            i = j;
                            j = tmp;
                        }
                        if (_label[_inblossom[i]] != 1) throw new Exception("Assert Error 295");
                        _queue.RemoveLast();
                    }
                    else if (deltatype == 3)
                    {
                        _allowedge[deltaEdge] = true;
                        var i = _myEdges[deltaEdge][0];
                        var j = _myEdges[deltaEdge][1];
                        var wt = _myEdges[deltaEdge][2];
                        if (_label[_inblossom[i]] != 1) throw new Exception("Assert error 304");
                        _queue.RemoveLast();
                    }
                    else if (deltatype == 4)
                        ExpandBlossom(deltaBlossom, false);
                }
                if (!augmented)
                    break;
                for (var b = _amountOfNodes; b < 2 * _amountOfNodes; b++)
                {
                    if (_blossomparent[b] == -1 && _blossombase[b] >= 0 && _label[b] == 1 && _dualvar[b] == 0)
                        ExpandBlossom(b, true);
                }
            }
            //TODO CHECKOPTUMIM

            for (var n = 0; n < _amountOfNodes; n++)
            {
                if (_mate[n] >= 0)
                    _mate[n] = _endpoint[_mate[n]];
            }
            for (var n = 0; n < _amountOfNodes; n++)
            {
                if (!(_mate[n] == -1 || _mate[_mate[n]] == n)) throw new Exception("Asser error");
            }
            return _mate.ToList();
        }

        private static int DivideAndFloor(int i, int j)
        {
            return (int) Math.Floor((float) i / j);
        }

        private static int IndexOf(MyList<int> elems, int i)
        {
            for (var index = 0; index < elems.Size(); index++)
            {
                if (elems[index] == i)
                    return index;
            }
            throw new Exception("Element not found");
        }

        private static MyList<int> Skip(MyList<int> v, int t)
        {
            var elems = new MyList<int>();
            for (var j = t; j < v.Size(); j++)
            {
                elems.Add(v[j]);
            }
            return elems;
        }

        private static MyList<int> Take(MyList<int> v, int t)
        {
            var elems = new MyList<int>();
            for (var j = 0; j < t; j++)
            {
                elems.Add(v[j]);
            }
            return elems;
        }


        private static void AssignLabel(int w, int t, int p)
        {
            var b = _inblossom[w];
            if (!(_label[w] == 0 && _label[b] == 0))
                throw new Exception("Assert Error");
            _label[w] = t;
            _label[b] = t;
            _labelend[w] = p;
            _labelend[b] = p;
            _bestedge[w] = -1;
            _bestedge[b] = -1;

            switch (t)
            {
                case 1:
                    var list = BlossemLeaves(b);
                    foreach (var t1 in list)
                    {
                        _queue.Add(t1);
                    }
                    break;
                case 2:
                    var Base = _blossombase[b];
                    if (!(_mate[Base] >= 0)) throw new Exception("Assert Error");
                    AssignLabel(_endpoint[_mate[Base]], 1, _mate[Base] ^ 1);
                    break;
            }
        }

        private static List<int> BlossemLeaves(int b)
        {
            var list = new List<int>();
            if (b < _amountOfNodes)
            {
                list.Add(b);
            }
            else
            {
                for (var i = 0; i < _blossomchilds[b].Count; i++)
                {
                    var t = _blossomchilds[b][i];
                    if (t < _amountOfNodes) list.Add(t);
                    else
                    {
                        var vs = BlossemLeaves(t);
                        list.AddRange(vs.Select(t1 => vs[i]));
                    }
                }
            }
            return list;
        }

        private static int Slack(int k)
        {
            return _dualvar[_myEdges[k][0]] + _dualvar[_myEdges[k][1]] - 2 * _myEdges[k][2];
        }


        private static int ScanBlossom(int v, int w)
        {
            var path = new MyList<int>();
            var Base = -1;
            while (v != -1 || w != -1)
            {
                var b = _inblossom[v];
                if (Convert.ToBoolean(_label[b] & 4))
                {
                    Base = _blossombase[b];
                    break;
                }
                if (_label[b] != 1) throw new Exception("Assert Error");
                path.Add(b);
                _label[b] = 5;

                if (_labelend[b] != _mate[_blossombase[b]]) throw new Exception("Assert Error");

                if (_labelend[b] == -1)
                    v = -1;
                else
                {
                    v = _endpoint[_labelend[b]];
                    b = _inblossom[v];
                    if (_label[b] != 2) throw new Exception("Assert Error");
                    if (_labelend[b] < 0) throw new Exception("Assert Error");
                    v = _endpoint[_labelend[b]];
                }
                if (w != -1)
                {
                    var tmp = v;
                    v = w;
                    w = tmp;
                }
            }
            for (var i = 0; i < path.Size(); i++)
            {
                _label[path[i]] = 1;
            }
            return Base;
        }

        private static void AddBlossom(int Base, int k)
        {
            var v = _myEdges[k][0];
            var w = _myEdges[k][1];
            var weight = _myEdges[k][2];

            var bb = _inblossom[Base];
            var bv = _inblossom[v];
            var bw = _inblossom[w];

            var b = _unusedblossoms[_unusedblossoms.Size() - 1];
            _unusedblossoms.RemoveLast();

            _blossombase[b] = Base;
            _blossomparent[b] = -1;
            _blossomparent[bb] = b;

            _blossomchilds[b] = new MyList<int>();

            _blossomdps[b] = new MyList<int>();

            while (bv != bb)
            {
                _blossomparent[bv] = b;
                _blossomchilds[b].Add(bv);
                _blossomdps[b].Add(_labelend[bv]);

                if (!(_label[bv] == 2 || _label[bv] == 1 && _labelend[bv] == _mate[_blossombase[bv]]))
                    throw new Exception("Assert Error");
                if (_labelend[bv] < 0) throw new Exception("Assert Error");

                v = _endpoint[_labelend[bv]];
                bv = _inblossom[v];
            }


            _blossomchilds[b].Add(bb);
            _blossomchilds[b].Reverse();
            _blossomdps[b].Reverse();
            //std::reverse(blossomchilds[b].begin(), blossomchilds[b].end());
            //std::reverse(blossomdps[b].begin(), blossomdps[b].end());
            _blossomdps[b].Add(2 * k);

            while (bw != bb)
            {
                _blossomparent[bw] = b;
                _blossomchilds[b].Add(bw);
                _blossomdps[b].Add(_labelend[bw] ^ 1);

                if (!(_label[bw] == 2 || _label[bw] == 1 && _labelend[bw] == _mate[_blossombase[bw]]))
                    throw new Exception("Assert Error");
                if (_labelend[bw] < 0) throw new Exception("Assert Error row 306");

                w = _endpoint[_labelend[bw]];
                bw = _inblossom[w];
            }

            if (_label[bb] != 1) throw new Exception("Assert Error row 312");
            _label[b] = 1;
            _labelend[b] = _labelend[bb];

            _dualvar[b] = 0;


            var vs = BlossomLeaves(b);
            for (var vi = 0; vi < vs.Size(); vi++)
            {
                var vv = vs[vi];
                if (_label[_inblossom[vv]] == 2)
                    _queue.Add(vv);
                _inblossom[vv] = b;
            }


            var bestedgeto = new MyList<int>();
            for (var i = 0; i < 2 * _amountOfNodes; i++)
            {
                bestedgeto.Add(-1);
            }

            for (var i = 0; i < _blossomchilds[b].Size(); i++)
            {
                var bvv = _blossomchilds[b][i];
                var nblists = new MyList<MyList<int>>();


                if (_blossombestedges[bvv].Size() == 0)
                {
                    var vss = BlossomLeaves(bv);
                    for (var m = 0; m < vss.Size(); m++)
                    {
                        nblists.Add(new MyList<int>());
                        var vv = vss[m];

                        for (var j = 0; j < _neightbend[v].Count; j++)
                        {
                            var p = _neightbend[vv][j];
                            nblists[m].Add(DivideAndFloor(p, 2)); //row 322
                        }
                    }
                }
                else
                {
                    nblists.Add(_blossombestedges[bv]);
                }
                for (var i1 = 0; i1 < nblists.Size(); i1++)
                {
                    var nblist = nblists[i1];
                    for (var j1 = 0; j1 < nblist.Size(); j1++)
                    {
                        var kk = nblist[j1];

                        var ii = _myEdges[kk][0];
                        var j = _myEdges[kk][1];
                        var wt = _myEdges[kk][2];

                        if (_inblossom[j] == b)
                        {
                            var tmp = ii;
                            ii = j;
                            j = tmp;
                        }
                        var bj = _inblossom[j];
                        if (bj != b && _label[bj] == 1 && (bestedgeto[bj] == -1 || Slack(kk) < Slack(bestedgeto[bj])))
                        {
                            bestedgeto[bj] = kk;
                        }
                    }
                }
                _blossombestedges[bv] = new MyList<int>();
                _bestedge[bv] = -1;
            }
            var ks = new MyList<int>();
            for (var i = 0; i < bestedgeto.Size(); i++)
            {
                if (bestedgeto[i] != -1) ks.Add(bestedgeto[i]);
            }
            _blossombestedges[b] = ks;
            _bestedge[b] = -1;

            for (var i = 0; i < _blossombestedges[b].Size(); i++)
            {
                var ka = _blossombestedges[b][i];
                if (_bestedge[b] == -1 || Slack(ka) < Slack(_bestedge[b]))
                {
                    _bestedge[b] = ka;
                }
            }
        }

        private static MyList<int> BlossomLeaves(int b)
        {
            var lst = new MyList<int>();
            if (b < _amountOfNodes)
            {
                lst.Add(b);
            }
            else
            {
                for (var i = 0; i < _blossomchilds[b].Size(); i++)
                {
                    var t = _blossomchilds[b][i];
                    if (t < _amountOfNodes)
                    {
                        lst.Add(t);
                    }
                    else
                    {
                        var vs = BlossomLeaves(t);
                        for (var j = 0; j < vs.Size(); j++)
                        {
                            lst.Add(vs[j]);
                        }
                    }
                }
            }
            return lst;
        }

        private static void AugmentMatching(int k)
        {
            var v = _myEdges[k][0];
            var w = _myEdges[k][1];
            var wt = _myEdges[k][2];

            var ss = new MyList<int>();
            var ps = new MyList<int>();

            ss.Add(v);
            ss.Add(w);
            ps.Add(2 * k + 1);
            ps.Add(2 * k);

            for (var i = 0; i < 2; i++)
            {
                var s = ss[i];
                var p = ps[i];
                while (true)
                {
                    var bs = _inblossom[s];
                    if (_label[bs] != 1) throw new Exception("Assert Error");
                    if (_labelend[bs] != _mate[_blossombase[bs]]) throw new Exception("Assert Error");

                    if (bs >= _amountOfNodes)
                        AugmentBlossom(bs, s);
                    _mate[s] = p;
                    if (_labelend[bs] == -1)
                        break;

                    var t = _endpoint[_labelend[bs]];
                    var bt = _inblossom[t];
                    if (_label[bt] != 2) throw new Exception("Assert Error");
                    if (_labelend[bt] < 0) throw new Exception("Assert Error");
                    s = _endpoint[_labelend[bt]];
                    var j = _endpoint[_labelend[bt] ^ 1];
                    if (_blossombase[bt] != t) throw new Exception("Assert Error");
                    if (bt >= _amountOfNodes)
                        AugmentBlossom(bt, j);
                    _mate[j] = _labelend[bt];
                    p = _labelend[bt] ^ 1;
                }
            }
        }

        private static void AugmentBlossom(int b, int v)
        {
            int t = v, p;
            while (_blossomparent[t] != b)
            {
                t = _blossomparent[t];
            }
            if (t >= _amountOfNodes)
            {
                AugmentBlossom(t, v);
            }
            int jstep, endptrick;
            var i = IndexOf(_blossomchilds[b], t);
            var j = i;
            if (Convert.ToBoolean(i & 1))
            {
                j -= _blossomchilds[b].Size();
                jstep = 1;
                endptrick = 0;
            }
            else
            {
                jstep = -1;
                endptrick = 1;
            }
            while (j != 0)
            {
                j += jstep;
                t = _blossomchilds[b][j];
                p = _blossomdps[b][j - endptrick] ^ endptrick;
                if (t >= _amountOfNodes)
                {
                    AugmentBlossom(t, _endpoint[p]);
                }
                j += jstep;
                t = _blossomchilds[b][j];
                if (t >= _amountOfNodes)
                {
                    AugmentBlossom(t, _endpoint[p ^ 1]);
                }
                _mate[_endpoint[p]] = p ^ 1;
                _mate[_endpoint[p ^ 1]] = p;
            }
            var lst1 = Skip(_blossomchilds[b], i);
            var lst2 = Take(_blossomchilds[b], i);
            for (var ii = 0; ii < lst2.Size(); ii++)
            {
                lst1.Add(lst2[ii]);
            }
            _blossomchilds[b] = lst1;


            var lst3 = Skip(_blossomdps[b], i);
            var lst4 = Take(_blossomdps[b], i);
            for (var ii = 0; ii < lst4.Size(); ii++)
            {
                lst3.Add(lst4[ii]);
            }
            _blossomdps[b] = lst3;
            _blossombase[b] = _blossombase[_blossomchilds[b][0]];
            if (_blossombase[b] != v)
                throw new Exception("Assert Error row 479");
        }

        private static void ExpandBlossom(int b, bool endstage)
        {
            for (var i = 0; i < _blossomchilds[b].Size(); i++)
            {
                var s = _blossomchilds[b][i];
                _blossomparent[s] = -1;
                if (s < _amountOfNodes && _dualvar[s] == 0)
                {
                    ExpandBlossom(s, endstage);
                }
                else
                {
                    var vs = BlossomLeaves(s);
                    for (var j = 0; j < vs.Size(); j++)
                    {
                        var v = vs[j];
                        _inblossom[v] = s;
                    }
                }
            }
            if (!endstage && _label[b] == 2)
            {
                if (_labelend[b] < 0) throw new Exception("Assert Error");

                var entrychild = _inblossom[_endpoint[_labelend[b] ^ 1]];
                var j = IndexOf(_blossomchilds[b], entrychild);
                int jstep, endptrick;
                if (Convert.ToBoolean(j & 1))
                {
                    j -= _blossomchilds[b].Size();
                    ;
                    jstep = 1;
                    endptrick = 0;
                }
                else
                {
                    jstep = -1;
                    endptrick = 1;
                }
                var p = _labelend[b];
                while (j != 0)
                {
                    _label[_endpoint[p ^ 1]] = 0;
                    _label[_endpoint[_blossomdps[b][j - endptrick] ^ endptrick]] = 0;
                    AssignLabel(_endpoint[p ^ 1], 2, p);
                    _allowedge[DivideAndFloor(_blossomdps[b][j - endptrick], 2)] = true; // row 391
                    j += jstep;
                    p = _blossomdps[b][j - endptrick] ^ endptrick;
                    _allowedge[DivideAndFloor(p, 2)] = true; // row 395
                    j += jstep;
                }
                var bv = _blossomchilds[b][j];
                _label[_endpoint[p ^ 1]] = 2;
                _label[bv] = 2;
                _labelend[_endpoint[p ^ 1]] = p;
                _labelend[bv] = p;
                _bestedge[bv] = -1;
                j += jstep;
                while (_blossomchilds[b][j] != entrychild)
                {
                    bv = _blossomchilds[b][j];
                    if (_label[bv] == 1)
                    {
                        j += jstep;
                        continue;
                    }
                    var vs = BlossomLeaves(bv);
                    var v = 0;
                    for (var i = 0; i < vs.Size(); i++)
                    {
                        v = vs[i];
                        if (_label[v] != 0) break;
                    }
                    if (_label[v] != 0)
                    {
                        if (_label[v] != 2) throw new Exception("Assert Error");
                        if (_inblossom[v] != bv) throw new Exception("Assert Error");
                        _label[v] = 0;
                        _label[_endpoint[_mate[_blossombase[bv]]]] = 0;
                        AssignLabel(v, 2, _labelend[v]);
                    }
                    j += jstep;
                }
            }

            _label[b] = -1;
            _labelend[b] = -1;
            _blossomchilds[b] = new MyList<int>();
            _blossomdps[b] = new MyList<int>();
            _blossombase[b] = -1;
            _blossombestedges[b] = new MyList<int>();
            _bestedge[b] = -1;
            _unusedblossoms.Add(b);
        }
    }
}