using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common
{
    /// <summary>
    /// �ȽǺ��߼����㷨��
    /// </summary>
    public class RhumbLineArgorithm 
    {       
        /// <summary>
        /// �����������롣
        /// see http://williams.best.vwh.net/avform.htm#Rhumb
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static double CalculateDistance(PointCoordinate firstPoint, PointCoordinate endPoint)
        {
            double R = 6371;
            double lat2 = GreatCircleArgorithm.ConvertToDeg(endPoint.Latitide);
            double lon2 = GreatCircleArgorithm.ConvertToDeg(endPoint.Longitude);
            double lat1 = GreatCircleArgorithm.ConvertToDeg(firstPoint.Latitide);
            double lon1 = GreatCircleArgorithm.ConvertToDeg(firstPoint.Longitude);

            double dLat = GreatCircleArgorithm.ConvertToARC(lat2 - lat1);
            double dLon = Math.Abs(GreatCircleArgorithm.ConvertToARC(lon2 - lon1));

            double dPhi = Math.Log(Math.Tan(GreatCircleArgorithm.ConvertToARC(lat2) / 2 + Math.PI / 4) / Math.Tan(GreatCircleArgorithm.ConvertToARC(lat1) / 2 + Math.PI / 4));
            double q = (Math.Abs(dLat) > 0.00000001) ? dLat / dPhi : Math.Cos(GreatCircleArgorithm.ConvertToARC(lat1));

            if (dLon > Math.PI)
                dLon = 2 * Math.PI - dLon;

            double d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon);
            return d * R;        
        }

        /// <summary>
        /// ��������ĽǶȡ�
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns>����60���ƵĶȷ���Ƕ� </returns>
        public static double CalculateAngle(PointCoordinate firstPoint, PointCoordinate endPoint)
        {
            double lat2 = GreatCircleArgorithm.ConvertToDeg(endPoint.Latitide);
            double lon2 = GreatCircleArgorithm.ConvertToDeg(endPoint.Longitude);
            double lat1 = GreatCircleArgorithm.ConvertToDeg(firstPoint.Latitide);
            double lon1 = GreatCircleArgorithm.ConvertToDeg(firstPoint.Longitude);

            double dLon = GreatCircleArgorithm.ConvertToARC((lon2 - lon1));
            double dPhi = Math.Log(Math.Tan(GreatCircleArgorithm.ConvertToARC(lat2) / 2 + Math.PI / 4) / Math.Tan(GreatCircleArgorithm.ConvertToARC(lat1) / 2 + Math.PI / 4));

            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);

            double angle = Math.Atan2(dLon, dPhi);
            return GreatCircleArgorithm.ConvertToDMS((GreatCircleArgorithm.ConvertToDegree(angle) + 360) % 360);
            
  
        }

        /// <summary>
        /// �����Ѿ��㡢��λ�ͽǶȣ�������һ�����ꡣ
        /// </summary>
        /// <param name="angle">���߽�(�ŷ��򣬻��ȵ�λ)</param>
        /// <param name="distance">����(����)</param>
        /// <param name="point">�ο���</param>
        /// <returns></returns>
        public static PointCoordinate CalcuateCoordinate(PointCoordinate point, double angle,
            double distance)
        {
            PointCoordinate p = new PointCoordinate();
            double R = 6371.0; // earth's mean radius in km
            double d = distance / R;

            double lat1 = GreatCircleArgorithm.ConvertToARC(GreatCircleArgorithm.ConvertToDeg(point.Latitide));
            double lon1 = GreatCircleArgorithm.ConvertToARC(GreatCircleArgorithm.ConvertToDeg(point.Longitude));

            double brng = GreatCircleArgorithm.ConvertToDeg(angle) * Math.PI / 180;   //����ת��Ϊ����

            double lat2 = lat1 + d * Math.Cos(brng);
            double dLat = lat2 - lat1;
            double dPhi = Math.Log(Math.Tan(lat2 / 2 + Math.PI / 4) / Math.Tan(lat1 / 2 + Math.PI / 4));
            double q = (Math.Abs(dLat) > 0.000000001) ? dLat / dPhi : Math.Cos(lat1);
            double dlon = d * Math.Sin(brng) / q;
            if (Math.Abs(lat2) > Math.PI / 2)
            {
                lat2 = lat2 > 0 ? Math.PI - lat2 : -Math.PI - lat2;
            }

            double lon2 = (lon1+ dlon + Math.PI) % (2 * Math.PI) - Math.PI;
            p.Latitide = GreatCircleArgorithm.ConvertToDMS(GreatCircleArgorithm.ConvertToDegree(lat2));
            p.Longitude = GreatCircleArgorithm.ConvertToDMS(GreatCircleArgorithm.ConvertToDegree(lon2));

            return p;
        }
    }
}
