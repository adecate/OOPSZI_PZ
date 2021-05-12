using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web_Service.Models;

namespace Web_Service.Helpers
{
    static public class JWriter<T>
    {
        static public string Write(in T collection, string current_data = null)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            try
            {
                using (Newtonsoft.Json.JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartArray();

                    foreach (var item in (System.Collections.IList)collection)
                    {
                        writer.WriteStartObject();


                        writer.WritePropertyName("CommonName");
                        writer.WriteValue((item as ParkingAreaInfo).CommonName);

                        writer.WritePropertyName("Location");
                        writer.WriteValue((item as ParkingAreaInfo).Location);


                        writer.WritePropertyName("BalanceholderPhone");
                        writer.WriteStartArray();
                        foreach (var element in (item as ParkingAreaInfo).BalanceholderPhone)
                        {
                            writer.WriteStartObject();

                            writer.WritePropertyName("BalanceholderPhone");
                            writer.WriteValue(element.BalanceholderPhone);


                            writer.WriteEndObject();
                        }
                        writer.WriteEnd();


                        writer.WritePropertyName("BalanceholderWebSite");
                        writer.WriteValue((item as ParkingAreaInfo).BalanceholderWebSite);

                        writer.WritePropertyName("NeighbourhoodPark");
                        writer.WriteValue((item as ParkingAreaInfo).NeighbourhoodPark);

                        writer.WritePropertyName("HasWater");
                        writer.WriteValue((item as ParkingAreaInfo).HasWater);

                        writer.WritePropertyName("HasPlayground");
                        writer.WriteValue((item as ParkingAreaInfo).HasPlayground);

                        writer.WritePropertyName("HasSportground");
                        writer.WriteValue((item as ParkingAreaInfo).HasSportground);

                        writer.WritePropertyName("OperationOrganizationName");
                        writer.WriteValue((item as ParkingAreaInfo).OperationOrganizationName);


                        writer.WritePropertyName("WorkingHours");
                        writer.WriteStartArray();
                        foreach (var element in (item as ParkingAreaInfo).WorkingHours)
                        {
                            writer.WriteStartObject();

                            writer.WritePropertyName("DayOfWeek");
                            writer.WriteValue(element.DayOfWeek);

                            writer.WritePropertyName("Hours");
                            writer.WriteValue(element.Hours);

                            writer.WriteEndObject();
                        }
                        writer.WriteEnd();


                        writer.WritePropertyName("DepartamentalAffiliationComp");
                        writer.WriteValue((item as ParkingAreaInfo).DepartamentalAffiliationComp);

                        writer.WriteEndObject();
                    }

                    writer.WriteEnd();

                    if (current_data != "\r\n" && !string.IsNullOrEmpty(current_data))
                    {
                        JArray current_doc = JArray.Parse(current_data);

                        JArray new_data = JArray.Parse(sb.ToString());
                        var child_new_data = new_data.Children();

                        current_doc.Add(child_new_data);

                        return current_doc.ToString();

                    }

                    return sb.ToString();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

        }
    }
}