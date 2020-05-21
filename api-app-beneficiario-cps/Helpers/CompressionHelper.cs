using System.IO;
//Install-Package MarkerMetro.Unity.Ionic.Zlib
namespace api_app_beneficiario_cps.Helpers
{
    public class CompressionHelper
    {
        public static byte[] DeflateByte(byte[] str)
        {
            if (str == null) return null;
        
            using (var output = new MemoryStream())
            {
                using (
                    var compressor = new Ionic.Zlib.DeflateStream(
                    output, Ionic.Zlib.CompressionMode.Compress,
                    Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(str, 0, str.Length);
                }
                return output.ToArray();
            }
        }
    }
}