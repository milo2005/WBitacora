using Bitacora.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Bitacora.Net
{
    public class ZonasApi
    {
        const string URL= "http://localhost/turismo/public/index.php/zonas";

        HttpConnection cx;

        public ZonasApi() {
            cx = new HttpConnection();
        }

        public async Task<List<Zona>> getZonas() {
            List<Zona> data = new List<Zona>();

            string json = await cx.requestByGet(URL);

            JsonArray zonas = JsonArray.Parse(json);
            for (uint i =0; i< zonas.Count; i++) {
                JsonObject o = zonas.GetObjectAt(i);
                Zona z = new Zona();
                z.Nombre = o["nombre"].GetString();
                z.Descripcion = o["descripcion"].GetString();
                z.Imagen = o["imagen"].GetString();
                z.Direccion = o["direccion"].GetString();
                data.Add(z);
            }
            return data;
        }

        public async Task<bool> insertZona(Zona z, StorageFile image) {
            JsonObject json = new JsonObject();
            json.Add("nombre", JsonValue.CreateStringValue(z.Nombre));
            json.Add("descripcion", JsonValue.CreateStringValue(z.Descripcion));
            json.Add("direccion", JsonValue.CreateStringValue(z.Direccion));

            string base64 = await imageToBase64(image);
            json.Add("imagen", JsonValue.CreateStringValue(base64));

            string rta = await cx.requestByJsonPost(URL, json.ToString());

            JsonObject rtaJ = JsonObject.Parse(rta);
            if (rtaJ["status"].GetString() == "OK")
            {
                z.Imagen = rtaJ["img"].GetString();
                return true;
            }
            else {
                return false;    
            }            
        }

        private async Task<string> imageToBase64(StorageFile image) {
            IRandomAccessStream random = await RandomAccessStreamReference.CreateFromFile(image).OpenReadAsync();

            var buffer = await Windows.Storage.FileIO.ReadBufferAsync(image);
            DataReader reader = DataReader.FromBuffer(buffer);

            byte[] data = new byte[buffer.Length];
            reader.ReadBytes(data);


            //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(random);
            
            //PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            //byte[] imgD = pixelData.DetachPixelData();

            return Convert.ToBase64String(data);
        }

    }
}
