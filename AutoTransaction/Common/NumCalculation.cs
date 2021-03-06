﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTransaction.Common
{
  public class NumCalculation
    {
        /// <summary>
        /// 获得买入数量
        /// </summary>
        /// <param name="_datalist">对应股票数据</param>
        /// <param name="_canBuyNum">可买数量</param>
        /// <returns></returns>
        public static int GetBuyNum(string [] _dataList,int _canBuyNum)
        {
            //盘口数量 = 卖1量 + 卖2量 + 卖3量
            int HandicapNum = (Convert.ToInt32(_dataList[20]) + Convert.ToInt32(_dataList[22]) + Convert.ToInt32(_dataList[24])) / 100;
            if (_canBuyNum > 20000)
                _canBuyNum = 20000;
            if (_canBuyNum < HandicapNum)
                return _canBuyNum;
            else
                return HandicapNum;
        }

        /// <summary>
        /// 获得卖出数量
        /// </summary>
        /// <param name="_positionlist">对应股票的持仓单数据</param>
        /// <param name="_datalist">对应股票的实时数据</param>
        /// <param name="_a_param">参数A</param>
        /// <param name="_b_param">参数B</param>
        /// <param name="_c_param">参数C</param>
        /// <returns></returns>
        public static string GetSaleNum(string  [] _positionlist, string [] _datalist, double[] _a_param, double[] _b_param, double[]  _c_param)
        {
            //（DYNAINFO(5)- DYNAINFO(7)）*100/ DYNAINFO(7) 当天最高回落  (最高价 - 现价) * 100 / 现价
            var B = (Convert.ToDouble(_datalist[4]) - Convert.ToDouble(_datalist[3])) * 100 / Convert.ToDouble(_datalist[3]);
            //盈亏比例 = (现价 - 成本价）/成本价
            var Ratio =(Int32)Convert.ToDouble( _positionlist[9]);
            if (Ratio > 0)
            {
                //当盈亏比例〉A1%且盈亏比例<A2%时，当天最高价回落B1%时卖出可用股份C1%股份
                if ( (Ratio * 0.01) > (_a_param[0] * 0.01) && Ratio * 0.01 <= (_a_param[1] * 0.01) && B >= (_b_param[0] * 0.01))
                {
                    return Convert.ToInt32(_c_param[0] * 0.01 * Convert.ToDouble(_positionlist[3])).ToString();
                }
                //当盈亏比例〉A2%且盈亏比例<A3%时，当天最高价回落B2%时卖出可用股份C2%股份
                else if ((Ratio * 0.01) > (_a_param[1] * 0.01) && Ratio * 0.01 <= (_a_param[2] * 0.01) && B >= (_b_param[1] * 0.01))
                {
                    return Convert.ToInt32(_c_param[1] * 0.01 * Convert.ToDouble(_positionlist[3])).ToString();
                }
                //当盈亏比例〉A3%时，当天最高价回落B3%时卖出可用股份100%的全部股份
                else if ((Ratio * 0.01) > (_a_param[2] * 0.01) && B >= (_b_param[2] * 0.01))
                {
                    
                    return Convert.ToInt32(_positionlist[3]).ToString();
                }
                //其他情况返回0
                else
                {
                    return "0";
                }
            }
            else
            {
                //当盈亏比例<-A4%且盈亏比例>-A5%时，卖出可用股份C3%股份
                if ((Ratio * 0.01) > (-_a_param [4] * 0.01) && (Ratio * 0.01 < ( -_a_param[3] * 0.01)))
                {
                    //return "1350";
                    return Convert.ToInt32(_c_param[2] * 0.01 * Convert.ToDouble(_positionlist[3])).ToString();
                }
                //当盈亏比例<-A5%时，卖出可用股份C4%股份
                else if ((Ratio * 0.01) <= -_a_param[4] * 0.01)
                {
                    //return "1350";
                    return Convert.ToInt32(_c_param[3] * 0.01 * Convert.ToDouble(_positionlist[3])).ToString();
                }
                //其他情况返回0
                else
                {
                    return "0";
                }
            }
           
            
        }


    }
}
