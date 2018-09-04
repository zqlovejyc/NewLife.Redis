﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NewLife.Caching
{
    /// <summary>Set结构</summary>
    /// <typeparam name="T"></typeparam>
    public class RedisSet<T> : RedisBase, ICollection<T>
    {
        #region 属性
        #endregion

        #region 构造
        /// <summary>实例化</summary>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        public RedisSet(Redis redis, String key) : base(redis, key) { }
        #endregion

        #region 列表方法
        /// <summary>个数</summary>
        public Int32 Count => Execute(r => r.Execute<Int32>("SCARD", Key));

        Boolean ICollection<T>.IsReadOnly => false;

        /// <summary>添加元素在后面</summary>
        /// <param name="item"></param>
        public void Add(T item) => SAdd(item);

        /// <summary>清空列表</summary>
        public void Clear() => throw new NotSupportedException();

        /// <summary>是否包含指定元素</summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean Contains(T item) => Execute(r => r.Execute<Boolean>("SISMEMBER", Key, item));

        /// <summary>复制到目标数组</summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            var count = array.Length - arrayIndex;

            var arr = GetAll();
            arr.CopyTo(array, arrayIndex);
        }

        /// <summary>删除指定元素</summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean Remove(T item) => throw new NotSupportedException();

        /// <summary>遍历</summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() => GetAll().ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region 高级操作
        /// <summary>批量添加</summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public Int32 SAdd(params T[] members)
        {
            var args = new List<Object>
            {
                Key
            };
            foreach (var item in members)
            {
                args.Add(item);
            }

            return Execute(r => r.Execute<Int32>("SADD", args.ToArray()));
        }

        /// <summary>批量删除</summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public Int32 SDel(params T[] members)
        {
            var args = new List<Object>
            {
                Key
            };
            foreach (var item in members)
            {
                args.Add(item);
            }

            return Execute(r => r.Execute<Int32>("SREM", args.ToArray()));
        }

        /// <summary>获取所有元素</summary>
        /// <returns></returns>
        public T[] GetAll() => Execute(r => r.Execute<T[]>("SMEMBERS", Key));

        /// <summary>将member从source集合移动到destination集合中</summary>
        /// <param name="dest"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public T[] Move(String dest, T member) => Execute(r => r.Execute<T[]>("SMOVE", Key, dest, member));

        /// <summary>随机获取多个</summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] RandomGet(Int32 count) => Execute(r => r.Execute<T[]>("SRANDMEMBER", Key, count));

        /// <summary>随机获取并弹出</summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] Pop(Int32 count) => Execute(r => r.Execute<T[]>("SPOP", Key, count));
        #endregion
    }
}