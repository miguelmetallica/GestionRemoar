using Gestion.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion.Web.Data
{
    public class CombosRepository : ICombosRepository
    {
        public CombosRepository()
        {
        }

        public List<CombosItems> GetAños()
        {
            var obj = new List<CombosItems>();

            for (var i = 0; i <= 14; i++)
            {
                obj.Add(new CombosItems
                {
                    Value = (DateTime.Now.Year + i).ToString().PadLeft(4, '0'),
                    Text = (DateTime.Now.Year + i).ToString().PadLeft(4, '0'),
                });
            }

            return obj;//.OrderBy(d => d.Display).ToList();

        }
        public List<CombosItems> GetMeses()
        {
            var obj = new List<CombosItems>();

            for (var i = 1; i <= 12; i++)
            {
                obj.Add(new CombosItems
                {
                    Value = i.ToString().PadLeft(2, '0'),
                    Text = i.ToString().PadLeft(2, '0'),
                });
            }

            return obj;//.OrderBy(d => d.Display).ToList();

        }
    }
}
