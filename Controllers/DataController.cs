using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Service.Models;
using Web_Service.Helpers;

namespace Web_Service.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ParkingAreasdbMainContext db;
        private string datanumber = "1465",
                       api_key = "fbba147ec52998960d3ced76a2f99bb8";

        public DataController(ParkingAreasdbMainContext context)
        {
            db = context;
        }

        [HttpGet("api/[controller]/Show")]
        public async Task<IActionResult> Show()
        {
            return Ok(JWriter<List<ParkingAreaInfo>>.Write(await db.ParkingAreas.Include(m => m.BalanceholderPhone).Include(s => s.WorkingHours).ToListAsync()));
        }

        [HttpGet("api/[controller]/Create")]
        public async Task<IActionResult> Create() 
        {
            try
            {
                string data = Req_h.Resp($"https://apidata.mos.ru/v1/datasets/{datanumber}/rows/?api_key={api_key}");
                var Deser_Obj = JDeserializer<List<ParkingArea>>.Deser(data);

                db.ParkingAreas.RemoveRange(db.ParkingAreas);

                foreach (var item in Deser_Obj)
                {
                    await db.AddAsync(item.Cells);
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return Ok("Данные записаны");
        }

        [HttpGet("api/[controller]/Update")]
        public async Task<IActionResult> Update()
        {
            int current_count = await db.ParkingAreas.CountAsync(), count = 0;

            try
            {
                count = int.Parse(Req_h.Resp($"https://apidata.mos.ru/v1/datasets/{datanumber}/count/?api_key={api_key}"));

                if (current_count < count)
                {
                    string data = Req_h.Resp($"https://apidata.mos.ru/v1/datasets/{datanumber}/rows/?api_key={api_key}&$orderby=global_id%20desc&$top=" + (count - current_count));

                    var Deser_Obj = JDeserializer<List<ParkingArea>>.Deser(data);

                    foreach (var item in Deser_Obj)
                    {
                        await db.AddAsync(item.Cells);
                    }

                    await db.SaveChangesAsync();

                    return Ok("Данные обновлены");
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            return Ok("Данные не требуют обновления");
        }

        [HttpGet("api/[controller]/Export")]
        public async Task<IActionResult> Export()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("DataFile.json", false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(JWriter<List<ParkingAreaInfo>>.Write(await db.ParkingAreas.Include(m => m.BalanceholderPhone).Include(s => s.WorkingHours).ToListAsync()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Ok("Данные экспортированны в формат .json");
        }

    }
}