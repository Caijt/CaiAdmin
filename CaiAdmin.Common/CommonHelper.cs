using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CaiAdmin.Common
{
    /// <summary>
    /// 公共助手
    /// </summary>
    public class CommonHelper
    {
        public static string ObjectToJsonString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, setting);
        }
        public static T JsonStringToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static object JsonStringToObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
        public static string CamelCaseToLowerUnderScore(string name)
        {
            return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// 计算两个点之间的距离，单位米
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat, double lng, double lat2, double lng2)
        {
            double EARTH_RADIUS = 6378.137;
            double radLat1 = lat * Math.PI / 180.0;
            double radLat2 = lat2 * Math.PI / 180.0;
            var a = radLat1 - radLat2;
            var b = (lng * Math.PI / 180.0) - (lng2 * Math.PI / 180.0);
            var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 1000);
            return s;
        }
        /// <summary>
        /// 获取DisplayNameAttribute特性的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(object value)
        {
            return (value?.GetType()?.GetField(value?.ToString())?.GetCustomAttributes(typeof(DisplayNameAttribute), false)?.FirstOrDefault() as DisplayNameAttribute)?.DisplayName ?? value?.ToString();

        }
        /// <summary>
        /// 获取DescriptionAttribute特性的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(object value)
        {
            return (value?.GetType()?.GetField(value?.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false)?.FirstOrDefault() as DescriptionAttribute)?.Description ?? value?.ToString();
        }

        /// <summary>
        /// 增量保存实体数组，传入一个新数组数据跟一个数据库的旧数组数据，把数据库的数据同步为新数据，需在调用后手动进行SaveChange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targeEntities">新数组数据</param>
        /// <param name="originEntities">旧数组数据</param>
        /// <param name="condition">判断条件，第一个是新对象，第二个是旧对象</param>
        /// <param name="afterUpdate">更新后实体后事件</param>
        /// <param name="afterDelete">删除实体后事件</param>
        public static void CollectionChange<T>(IEnumerable<T> targeEntities,
             IEnumerable<T> originEntities,
             Func<T, T, bool> condition,
             Action<List<T>> insertFunc,
             Action<List<T>> updateFunc = null,
             Action<List<T>> deleteFunc = null)
        {
            var insertData = new List<T>();
            var updateData = new List<T>();
            var deleteData = new List<T>();

            if (originEntities != null)
            {
                foreach (var oldEntity in originEntities)
                {
                    if (targeEntities == null || !targeEntities.Any(i =>
                    {
                        return condition(i, oldEntity);
                    }))
                    {
                        deleteData.Add(oldEntity);
                    }
                }
            }
            if (targeEntities != null)
            {
                foreach (var newEntity in targeEntities)
                {
                    T oldEntity = default(T);
                    if (originEntities != null && originEntities.Count() > 0)
                    {
                        oldEntity = originEntities.FirstOrDefault(e =>
                        {
                            return condition(newEntity, e);
                        });
                    }
                    if (oldEntity == null)
                    {
                        insertData.Add(newEntity);
                    }
                    else
                    {
                        updateData.Add(newEntity);
                    }
                }
            }

            if (insertData.Any())
            {
                insertFunc.Invoke(insertData);
            }
            if (updateData.Any())
            {
                updateFunc?.Invoke(updateData);
            }
            if (deleteData.Any())
            {
                deleteFunc?.Invoke(deleteData);
            }
        }
    }
}
