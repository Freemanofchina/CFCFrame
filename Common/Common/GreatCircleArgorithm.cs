using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common
{
    /// <summary>
    /// ��Բ�㷨,see http://www.movable-type.co.uk/scripts/latlong.html
    /// or http://williams.best.vwh.net/avform.htm
    /// </summary>
   public class GreatCircleArgorithm
    {
        const double R = 6378.137;  //����뾶km


        /// <summary>
        /// ת���ɻ���
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ConvertToARC(double d)
        {
            return d * Math.PI / 180.0;
        }

        ///<summary>
        /// ����ת����10���ƶ�
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ConvertToDegree(double d)
        {
            return d * 180 / Math.PI;
        }

        /// <summary>
        /// 60����ת����10���� ����Ϊ��λ��
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ConvertToDeg(double d)
        {
            bool b = true;
            if (d < 0)
            {
                b = false;
                d = Math.Abs(d);
            }

            //string ss = d.ToString("000.0000");

            //mqg��20110714���ӣ�����������Ϊ84��������λС��
            string ss = d.ToString("000.000000");

            string du = ss.Substring(0, 3);
            string fen = ss.Substring(4, 2);
            string miao = ss.Substring(6, 2);

            //double ddd = double.Parse(du) + double.Parse(fen) / 60 + double.Parse(miao) / 3600;

            //mqg��20110714���ӣ�����������Ϊ84��������λС��
            double miaowithdot = double.Parse(miao) + double.Parse(ss.Substring(8, 2)) / 100; //����С����
            double ddd = double.Parse(du) + double.Parse(fen) / 60 + miaowithdot / 3600;

            if (!b)
                ddd = -ddd;

            return ddd;

        }

        /// <summary>
        /// 10����ת����60���� (�ȷ���)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double ConvertToDMS(double d)
        {
            bool b = true;
            if (d < 0)
            {
                b = false;
                d = Math.Abs(d);
            }

            double du = (double)((int)d);  //�� 
            double fen = ((int)((d - du) * 60.0)) / 100.0;

            double miao = (d * 60.0 - (int)(d * 60)) * 60 / 10000.0;


            double ddd = du + fen + miao;

            if (!b)
                ddd = -ddd;

            return ddd;

        }
       
        public static double ToMeter(double inPut, string inUnit)
        {
            switch (inUnit)
            {
                case "M":
                    return inPut;
                case "KM":
                    return inPut * 1000;
                case "NM":
                    return inPut * 1852;
                case "FT":
                    return inPut * 0.3048;
                case "FL":
                    return inPut * 30.48;
                case "SM": //ʮ��
                    return inPut * 10;
                default:
                    throw new Exception("δ֪�ľ��뵥λ" + inUnit);
            }
        }

        public static double ToFeet(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 3.2808399;
        }

        public static double ToFL(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.032808399;
        }

        public static double ToNM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.00054;
        }

        public static double ToKM(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit) * 0.001;
        }

        public static double ToSTDMeter(double inPut, string inUnit)
        {
            return ToMeter(inPut, inUnit);
        }


        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
       public static double CalculateDistance(PointCoordinate firstPoint, PointCoordinate endPoint)
        {

            double lat2 = ConvertToARC(ConvertToDeg(endPoint.Latitide));
            double lat1 = ConvertToARC(ConvertToDeg(firstPoint.Latitide));
            double lon2 = ConvertToARC(ConvertToDeg(endPoint.Longitude));
            double lon1 = ConvertToARC(ConvertToDeg(firstPoint.Longitude));

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;



            /*
           *   var R = 6371; // earth's mean radius in km
var dLat = (lat2-lat1).toRad();
var dLon = (lon2-lon1).toRad();
lat1 = lat1.toRad(), lat2 = lat2.toRad();

var a = Math.sin(dLat/2) * Math.sin(dLat/2) +
        Math.cos(lat1) * Math.cos(lat2) * 
        Math.sin(dLon/2) * Math.sin(dLon/2);
var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
var d = R * c;
return d;
}
           * */
        }

        /// <summary>
        /// ���������Ƕ� 
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
       public static double CalculateAngle(PointCoordinate firstPoint, PointCoordinate endPoint)
        {

            double lat2 = ConvertToARC(ConvertToDeg(endPoint.Latitide));
            double lat1 = ConvertToARC(ConvertToDeg(firstPoint.Latitide));
            double lon2 = ConvertToARC(ConvertToDeg(endPoint.Longitude));
            double lon1 = ConvertToARC(ConvertToDeg(firstPoint.Longitude));

            double dLon = lon2 - lon1;
            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) -
                Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

            double angle = Math.Atan2(y, x);

            angle = (angle + Math.PI * 2) % (Math.PI * 2);
            return ConvertToDMS(ConvertToDegree(angle));
            /*
             *
             * lat1 = lat1.toRad(); lat2 = lat2.toRad();
  var dLon = (lon2-lon1).toRad();

  var y = Math.sin(dLon) * Math.cos(lat2);
  var x = Math.cos(lat1)*Math.sin(lat2) -
          Math.sin(lat1)*Math.cos(lat2)*Math.cos(dLon);
  return Math.atan2(y, x).toBrng();
             */



        }

        /// <summary>
        /// ��ĳ��֪��;��롢��λ������һ������
        /// </summary>
        /// <param name="point">��֪��</param>
        /// <param name="angle">��λ��</param>
        /// <param name="distance">���루���</param>
        /// <returns></returns>
       public static PointCoordinate CalcuateCoordinate(PointCoordinate point, double angle, double distance)
        {
            double lat1 = ConvertToARC(ConvertToDeg(point.Latitide));
            double lon1 = ConvertToARC(ConvertToDeg(point.Longitude));
            double brng = ConvertToARC(ConvertToDeg(angle));
            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance / R) +
                Math.Cos(lat1) * Math.Sin(distance / R) * Math.Cos(brng));

            double lon2 = lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(distance / R) * Math.Cos(lat1),
                Math.Cos(distance / R) - Math.Sin(lat1) * Math.Sin(lat2));

            lon2 = (lon2 + Math.PI) % (2 * Math.PI) - Math.PI;   //;normalise to -180...+180

            PointCoordinate p = new PointCoordinate();
            p.Longitude = ConvertToDMS(ConvertToDegree(lon2));
            p.Latitide = ConvertToDMS(ConvertToDegree(lat2));
            return p;

            /*
             *  var R = 6371; // earth's mean radius in km
  var lat1 = this.lat.toRad(), lon1 = this.lon.toRad();
  brng = brng.toRad();

  var lat2 = Math.asin( Math.sin(lat1)*Math.cos(d/R) + 
                        Math.cos(lat1)*Math.sin(d/R)*Math.cos(brng) );
  var lon2 = lon1 + Math.atan2(Math.sin(brng)*Math.sin(d/R)*Math.cos(lat1), 
                               Math.cos(d/R)-Math.sin(lat1)*Math.sin(lat2));
  lon2 = (lon2+Math.PI)%(2*Math.PI) - Math.PI;  // normalise to -180...+180

  if (isNaN(lat2) || isNaN(lon2)) return null;
  return new LatLon(lat2.toDeg(), lon2.toDeg());

             * */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="distance">��Point1�ľ���</param>
        /// <param name="angle">��Point2�ĽǶ�</param>
        /// <returns></returns>
       public static List<PointCoordinate> CalcuateCoordinate(PointCoordinate point1, PointCoordinate point2,
            double distance, double angle)
        {
            //�����㷨������Point1ΪD�㣬Point2ΪA�㣬δ֪��ΪB��

            List<PointCoordinate> points = new List<PointCoordinate>();



            double b = CalculateDistance(point1, point2) / R;   //�ȼ�����������
            double crs12 = CalculateAngle(point2, point1);       //�������ķ�λ��

            double crs_AB = ConvertToARC(ConvertToDeg(angle));


            double crs_AD = ConvertToARC(ConvertToDeg(crs12));

            //A��ļн�
            double A = (crs_AD - crs_AB + Math.PI) % (Math.PI * 2) - Math.PI;

            double r = Math.Sqrt((Math.Cos(b) * Math.Cos(b) + Math.Sin(b) * Math.Sin(b) * Math.Cos(A) * Math.Cos(A)));
            double p = Math.Atan2(Math.Sin(b) * Math.Cos(A), Math.Cos(b));
            double d = distance / R;

            if (Math.Cos(d) * Math.Cos(d) > r * r)
            {
                // points.Add(CalcuateCoordinate(point2, angle, p * R));
                throw new Exception("Ҫ����ĵ㲻����");
            }
            else
            {
                double dp = p + Math.Acos(Math.Cos(d) / r);
                if (dp > 0)
                {

                    points.Add(CalcuateCoordinate(point2, angle, dp * R));
                }
                dp = p - Math.Acos(Math.Cos(d) / r);
                if (dp > 0)
                {

                    points.Add(CalcuateCoordinate(point2, angle, dp * R));
                }
            }

            //foreach (PointCoordinate pp in points)
            //{
            //    double kkk = CalculateDistance(pp, point1);
            //    double aaa = CalculateAngle(point2, pp);
            //}

            if (points.Count == 0)
                throw new Exception("Ҫ����ĵ㲻����!");

            return points;

            /*
             * 
           Let points A and B define a great circle route and D be a third point. Find the points on the great circle through A and B that lie a distance d from D, if they exist.

               A = crs_AD - crs_AB

            ( crs_AB and crs_AD are the initial GC bearings from A to B and D, respectively. Compute using Course between points)

               b = dist_AD

            (dist_AD is the distance from A to D. Compute using Distance between points)

               r=(cos(b)^2+sin(b)^2*cos(A)^2)^(1/2)

            (acos(r) is the XTD)

               p=atan2(sin(b)*cos(A),cos(b))

            (p is the ATD)

               IF (cos(d)^2 > r^2) THEN
                  No points exist
               ELSE
                  Two points exist
                 dp = p +- acos(cos(d)/r)
               ENDIF

             dp are the distances of the desired points from A along AB. Their lat/lons can be computed using Lat/lon given radial and distance
             */
        }


        static double  sin(double x)
        {
            return Math.Sin(x);

        }
        static double  cos(double x)
        {
            return Math.Cos(x);
        }

        static double acos(double x)
        {
            return Math.Acos(x);
        }

        double pi = Math.PI;

        /// <summary>
        /// ��������������ꡣ
        /// �Ѿ��������꣬��֪δ֪�㵽2��ķ�λ��
        /// </summary>
        /// <param name="point1">��һ������</param>
        /// <param name="point2">�ڶ�������</param>
        ///<param name="angle1">����һ��ķ�λ��</param>
        /// <param name="angle2">���ڶ���ķ�λ��</param>
        /// <returns>����</returns>
       public static PointCoordinate CalcuateCoordinate(PointCoordinate point1, double angle1, PointCoordinate point2, double angle2)
        {
            PointCoordinate p = new PointCoordinate();

            //����㷨�����Ǹ���������������Ҫת��
            double lat1 = ConvertToARC(ConvertToDeg(point1.Latitide));
            double lon1 = ConvertToARC(ConvertToDeg(-point1.Longitude));
            double lat2 = ConvertToARC(ConvertToDeg(point2.Latitide));
            double lon2 = ConvertToARC(ConvertToDeg(-point2.Longitude));

            PointCoordinate tempPoint1 = CalcuateCoordinate(point1, angle1, 20000);   //��һ�㷽���ϵ���ʱ��            
            PointCoordinate tempPoint2 = CalcuateCoordinate(point2, angle2, 20000);   //�ڶ��㷽���ϵ���ʱ��

            angle1 = ConvertToARC(ConvertToDeg(angle1));
            angle2 = ConvertToARC(ConvertToDeg(angle2));

            double d = Math.PI / 2;

            double lat1_1 = Math.Asin(sin(lat1) * cos(d) + cos(lat1) * sin(d) * cos(angle1));

            double lon1_1 = Math.Atan2(sin(angle1) * sin(d) * cos(lat1), cos(d) - sin(lat1) * sin(lat1_1));
            lon1_1 = Modlon(lon1 - lon1_1);

            double lat2_1 = Math.Asin(sin(lat2) * cos(d) + cos(lat2) * sin(d) * cos(angle2));
            double lon2_1 = Math.Atan2(sin(angle2) * sin(d) * cos(lat2), cos(d) - sin(lat2) * sin(lat2_1));
            lon2_1 = Modlon(lon2 - lon2_1);



            double a1 = sin(lat1 - lat1_1) * sin((lon1 + lon1_1) / 2) * cos((lon1 - lon1_1) / 2)
                - sin(lat1 + lat1_1) * cos((lon1 + lon1_1) / 2) * sin((lon1 - lon1_1) / 2);

            double b1 = sin(lat1 - lat1_1) * cos((lon1 + lon1_1) / 2) * cos((lon1 - lon1_1) / 2) +
                sin(lat1 + lat1_1) * sin((lon1 + lon1_1) / 2) * sin((lon1 - lon1_1) / 2);

            double c1 = cos(lat1) * cos(lat1_1) * sin(lon1 - lon1_1);

            double a2 = sin(lat2 - lat2_1) * sin((lon2 + lon2_1) / 2) * cos((lon2 - lon2_1) / 2)
                - sin(lat2 + lat2_1) * cos((lon2 + lon2_1) / 2) * sin((lon2 - lon2_1) / 2);

            double b2 = sin(lat2 - lat2_1) * cos((lon2 + lon2_1) / 2) * cos((lon2 - lon2_1) / 2) +
                sin(lat2 + lat2_1) * sin((lon2 + lon2_1) / 2) * sin((lon2 - lon2_1) / 2);

            double c2 = cos(lat2) * cos(lat2_1) * sin(lon2 - lon2_1);


            double A = b1 * c2 - c1 * b2;   //v1(2) * v2(3) - v1(3) * v2(2)
            double B = c1 * a2 - a1 * c2;   //v1(3) * v2(1) - v1(1) * v2(3)
            double C = a1 * b2 - b1 * a2;  //v1(1) * v2(2) - v1(2) * v2(1))

            double Lat01 = Math.Atan2(C, Math.Sqrt(A * A + B * B));
            double Lon01 = Math.Atan2(-B, A);

            PointCoordinate r1 = new PointCoordinate();

            r1.Latitide = ConvertToDMS(ConvertToDegree(Lat01));
            r1.Longitude = -ConvertToDMS(ConvertToDegree(Lon01));


            PointCoordinate r2 = new PointCoordinate();

            r2.Latitide = ConvertToDMS(ConvertToDegree(-Lat01));
            r2.Longitude = -ConvertToDMS(ConvertToDegree(Modlon(Lon01 + Math.PI)));

            //�����㣬ֻ������ĵ㡣
            double ta = CalculateDistance(r1, point1);
            //double  ta1 = CalculateAngle(r2, point1);

            //mqg��20110831���ӣ�Ӧ���ü������ķ���
            double ta1 = CalculateDistance(r2, point1);
            if (ta > ta1)
                return r2;
            else
                return r1;
        }

       static double MD(double Y, double X)
        {
            // double a = Math.Floor(-0.2 / 4);
            double d = Y - X * Math.Floor(Y / X);
            return d;
        }

        /// <summary>
        /// ������������-180---180֮�� 
        /// </summary>
        /// <param name="lon"></param>
        /// <returns></returns>
       static double Modlon(double lon)
        {
            return MD(lon + Math.PI, 2 * Math.PI) - Math.PI;
        }

       
       /// <summary>
       /// ת���������ݾ�γ��
       /// </summary>
       /// <param name="lat">γ�Ⱥ���һλ�����N or S</param>
       /// <param name="lon">���Ⱥ���һλ�����E or W</param>
       /// <returns></returns>
       public static PointCoordinate GetPointCoordinate(string lat, string lon)
       {
           PointCoordinate p = new PointCoordinate();           

           if (lat.Substring(lat.Length - 1, 1) == "N")
           {
               lat = lat.Substring(0, lat.Length - 1);
               p.Latitide = double.Parse(lat) / 10000;
           }
           else
           {
               lat = lat.Substring(0, lat.Length - 1);
               p.Latitide = 0 - double.Parse(lat) / 10000;
           }

           
           if (lon.Substring(lon.Length - 1, 1) == "E")
           {
               lon = lon.Substring(0, lon.Length - 1);
               p.Longitude = double.Parse(lon) / 10000;
           }
           else
           {
               lon = lon.Substring(0, lon.Length - 1);
               p.Longitude = 0 - double.Parse(lon) / 10000;
           }

           return p;
       }

       /// <summary>
       /// �Ƚ��������꣬�ο��ݲ�ݲλ��
       /// </summary>
       /// <param name="longitude">��һ�㾭��</param>
       /// <param name="latitude">��һ��γ��</param>
       /// <param name="longi">�ڶ��㾭��</param>
       /// <param name="lat">�ڶ���γ��</param>
       /// <param name="errorRange">��Χ</param>
       /// <param name="overLat">1 γ�ȳ��� 0 ������</param>
       /// <param name="overLongi">1 ���ȳ��� 0 ������</param>
       /// <returns></returns>
       public static bool CompTwoPointCoord(double longitude, double latitude, double longi, double lat, string errorRange, ref string overLat, ref string overLongi)
       {
           //��һ�㾭��
           string currLongistr = (longitude / 10000).ToString("000.000000");
           double currLongiDeg = double.Parse(currLongistr.Substring(0, 3)); //��ǰ��������
           double currLongiFen = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 1, 2)); //��ǰ��������
           double currLongiMiao = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 3, 2)); //��ǰ���������
           double currLongiDotMiao = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 5, currLongistr.Length - (currLongistr.IndexOf('.') + 5))); //���С����,����û��
           if (currLongiDotMiao.ToString().Length == 1)
           {
               currLongiDotMiao = currLongiDotMiao / 10;
           }
           else if (currLongiDotMiao.ToString().Length == 2)
           {
               currLongiDotMiao = currLongiDotMiao / 100;
           }
           else
           {
               currLongiDotMiao = 0;
           }
           double tottleCurrLongiMiao = currLongiDeg * 60 * 60 + currLongiFen * 60 + currLongiMiao + currLongiDotMiao; //�ϼ����������������С����

           //��һ��γ��
           string currLatstr = (latitude / 10000).ToString("000.000000");
           double currLatDeg = double.Parse(currLatstr.Substring(0, 3)); //��ǰ��������
           double currLatFen = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 1, 2)); //��ǰ��������
           double currLatMiao = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 3, 2)); //��ǰ���������
           double currLatDotMiao = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 5, currLatstr.Length - (currLatstr.IndexOf('.') + 5))); //���С����,����û��
           if (currLatDotMiao.ToString().Length == 1)
           {
               currLatDotMiao = currLatDotMiao / 10;
           }
           else if (currLatDotMiao.ToString().Length == 2)
           {
               currLatDotMiao = currLatDotMiao / 100;
           }
           else
           {
               currLatDotMiao = 0;
           }
           double tottleCurrLatMiao = currLatDeg * 60 * 60 + currLatFen * 60 + currLatMiao + currLatDotMiao; //�ϼ����������������С����

           //�ڶ��㾭��
           string longistr = (longi / 10000).ToString("000.000000");
           double longiDeg = double.Parse(longistr.Substring(0, 3)); //��ǰ��������
           double longiFen = double.Parse(longistr.Substring(longistr.IndexOf('.') + 1, 2)); //��ǰ��������
           double longiMiao = double.Parse(longistr.Substring(longistr.IndexOf('.') + 3, 2)); //��ǰ���������
           double longiDotMiao = double.Parse(longistr.Substring(longistr.IndexOf('.') + 5, longistr.Length - (longistr.IndexOf('.') + 5)));
           if (longiDotMiao.ToString().Length == 1)
           {
               longiDotMiao = longiDotMiao / 10;
           }
           else if (longiDotMiao.ToString().Length == 2)
           {
               longiDotMiao = longiDotMiao / 100;
           }
           else
           {
               longiDotMiao = 0;
           }
           double tottlelongiMiao = longiDeg * 60 * 60 + longiFen * 60 + longiMiao + longiDotMiao; //�����������������С����

           //�ڶ���γ��
           string latstr = (lat / 10000).ToString("000.000000");
           double latDeg = double.Parse(latstr.Substring(0, 3)); //��ǰ��������
           double latFen = double.Parse(latstr.Substring(latstr.IndexOf('.') + 1, 2)); //��ǰ��������
           double latMiao = double.Parse(latstr.Substring(latstr.IndexOf('.') + 3, 2)); //��ǰ���������
           double latDotMiao = double.Parse(latstr.Substring(latstr.IndexOf('.') + 5, latstr.Length - (latstr.IndexOf('.') + 5)));
           if (latDotMiao.ToString().Length == 1)
           {
               latDotMiao = latDotMiao / 10;
           }
           else if (latDotMiao.ToString().Length == 2)
           {
               latDotMiao = latDotMiao / 100;
           }
           else
           {
               latDotMiao = 0;
           }
           double tottlelatMiao = latDeg * 60 * 60 + latFen * 60 + latMiao + latDotMiao; //�ϼ����������������С����

           if (Math.Abs(tottleCurrLongiMiao - tottlelongiMiao) > double.Parse(errorRange) || Math.Abs(tottleCurrLatMiao - tottlelatMiao) > double.Parse(errorRange))
           {
               overLongi = "1";
           }
           else
           {
               overLongi = "0";
           }

           if (Math.Abs(tottleCurrLatMiao - tottlelatMiao) > double.Parse(errorRange))
           {
               overLat = "1";
           }
           else
           {
               overLat = "0";
           }

           if (Math.Abs(tottleCurrLongiMiao - tottlelongiMiao) > double.Parse(errorRange) || Math.Abs(tottleCurrLatMiao - tottlelatMiao) > double.Parse(errorRange))
           {
               return true;
           }

           return false;
       }

       /// <summary>
       /// �ж������Ƿ��ظ�
       /// </summary>
       /// <param name="longitude">��һ�㾭��</param>
       /// <param name="latitude">��һ��γ��</param>
       /// <param name="longi">�ڶ��㾭��</param>
       /// <param name="lat">�ڶ���γ��</param>
       /// <param name="errorRange">��Χ��Ĭ��2��</param>
       /// <returns></returns>
       public static bool IsRepeatePoint(double longitude, double latitude, double longi, double lat, string errorRange)
       {
           //��һ�㾭��
           string currLongistr = (longitude / 10000).ToString("000.000000");
           double currLongiDeg = double.Parse(currLongistr.Substring(0, 3)); //��ǰ��������
           double currLongiFen = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 1, 2)); //��ǰ��������
           double currLongiMiao = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 3, 2)); //��ǰ���������
           double currLongiDotMiao = double.Parse(currLongistr.Substring(currLongistr.IndexOf('.') + 5, currLongistr.Length - (currLongistr.IndexOf('.') + 5))); //���С����,����û��
           if (currLongiDotMiao.ToString().Length == 1)
           {
               currLongiDotMiao = currLongiDotMiao / 10;
           }
           else if (currLongiDotMiao.ToString().Length == 2)
           {
               currLongiDotMiao = currLongiDotMiao / 100;
           }
           else
           {
               currLongiDotMiao = 0;
           }
           double tottleCurrLongiMiao = currLongiDeg * 60 * 60 + currLongiFen * 60 + currLongiMiao + currLongiDotMiao; //�ϼ����������������С����

           //��һ��γ��
           string currLatstr = (latitude / 10000).ToString("000.000000");
           double currLatDeg = double.Parse(currLatstr.Substring(0, 3)); //��ǰ��������
           double currLatFen = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 1, 2)); //��ǰ��������
           double currLatMiao = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 3, 2)); //��ǰ���������
           double currLatDotMiao = double.Parse(currLatstr.Substring(currLatstr.IndexOf('.') + 5, currLatstr.Length - (currLatstr.IndexOf('.') + 5))); //���С����,����û��
           if (currLatDotMiao.ToString().Length == 1)
           {
               currLatDotMiao = currLatDotMiao / 10;
           }
           else if (currLatDotMiao.ToString().Length == 2)
           {
               currLatDotMiao = currLatDotMiao / 100;
           }
           else
           {
               currLatDotMiao = 0;
           }
           double tottleCurrLatMiao = currLatDeg * 60 * 60 + currLatFen * 60 + currLatMiao + currLatDotMiao; //�ϼ����������������С����

           //�ڶ��㾭��
           string longistr = (longi / 10000).ToString("000.000000");
           double longiDeg = double.Parse(longistr.Substring(0, 3)); //��ǰ��������
           double longiFen = double.Parse(longistr.Substring(longistr.IndexOf('.') + 1, 2)); //��ǰ��������
           double longiMiao = double.Parse(longistr.Substring(longistr.IndexOf('.') + 3, 2)); //��ǰ���������
           double longiDotMiao = double.Parse(longistr.Substring(longistr.IndexOf('.') + 5, longistr.Length - (longistr.IndexOf('.') + 5)));
           if (longiDotMiao.ToString().Length == 1)
           {
               longiDotMiao = longiDotMiao / 10;
           }
           else if (longiDotMiao.ToString().Length == 2)
           {
               longiDotMiao = longiDotMiao / 100;
           }
           else
           {
               longiDotMiao = 0;
           }
           double tottlelongiMiao = longiDeg * 60 * 60 + longiFen * 60 + longiMiao + longiDotMiao; //�����������������С����

           //�ڶ���γ��
           string latstr = (lat / 10000).ToString("000.000000");
           double latDeg = double.Parse(latstr.Substring(0, 3)); //��ǰ��������
           double latFen = double.Parse(latstr.Substring(latstr.IndexOf('.') + 1, 2)); //��ǰ��������
           double latMiao = double.Parse(latstr.Substring(latstr.IndexOf('.') + 3, 2)); //��ǰ���������
           double latDotMiao = double.Parse(latstr.Substring(latstr.IndexOf('.') + 5, latstr.Length - (latstr.IndexOf('.') + 5)));
           if (latDotMiao.ToString().Length == 1)
           {
               latDotMiao = latDotMiao / 10;
           }
           else if (latDotMiao.ToString().Length == 2)
           {
               latDotMiao = latDotMiao / 100;
           }
           else
           {
               latDotMiao = 0;
           }
           double tottlelatMiao = latDeg * 60 * 60 + latFen * 60 + latMiao + latDotMiao; //�ϼ����������������С����

           if (Math.Abs(tottleCurrLongiMiao - tottlelongiMiao) < double.Parse(errorRange) * 60 && Math.Abs(tottleCurrLatMiao - tottlelatMiao) < double.Parse(errorRange) * 60)
           {
               return true;
           }

           return false;
       }


    }


    /// <summary>
    /// ��γ�������.
    /// </summary>
    public class PointCoordinate
    {
        double longitude;
        /// <summary>
        /// ����
        /// </summary>
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        double latitide;

        /// <summary>
        /// γ��
        /// </summary>
        public double Latitide
        {
            get { return latitide; }
            set { latitide = value; }
        }


    }
}
