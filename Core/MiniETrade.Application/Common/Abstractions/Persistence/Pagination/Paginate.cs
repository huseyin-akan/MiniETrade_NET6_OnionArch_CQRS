using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Repositories.Pagination;

public class Paginate<T> //TODO-HUS yukarıdaki abstract class olan PagebleQueryRequest arkadaşına taşıyalım bunu.
{
    public Paginate()
    {
        Items = Array.Empty<T>();  
    }

    public int Size { get; set; }
    public int Index { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public IList<T> Items { get; set; }
    public bool HasNext => Index + 1 < Pages;
    public bool HasPrevious => Index > 0;
}