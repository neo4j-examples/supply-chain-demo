using SpikeNeo4j.Helpers.IMathHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeNeo4j.Helpers
{
    public class MathHelper : IMathHelper
    {
        public decimal[] MidPoint(double lat1, double lon1, double lat2, double lon2)
        {
            double dLon = ConvertDegreesToRadians(lon2 - lon1);

            //convert to radians
            lat1 = ConvertDegreesToRadians(lat1);
            lat2 = ConvertDegreesToRadians(lat2);
            lon1 = ConvertDegreesToRadians(lon1);

            double Bx = Math.Cos(lat2) * Math.Cos(dLon);
            double By = Math.Cos(lat2) * Math.Sin(dLon);
            double lat3 = Math.Atan2(Math.Sin(lat1) + Math.Sin(lat2), Math.Sqrt((Math.Cos(lat1) + Bx) * (Math.Cos(lat1) + Bx) + By * By));
            double lon3 = lon1 + Math.Atan2(By, Math.Cos(lat1) + Bx);

            //print out in degrees
            decimal[] midPoint = { (decimal)ConvertRadiansToDegrees(lat3), (decimal)ConvertRadiansToDegrees(lon3) };
            return midPoint;
        }

        public double ConvertDegreesToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        public double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }
    }
}
