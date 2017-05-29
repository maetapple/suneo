using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if false
namespace Suneo.Resource
{
    //----------------------------------------------------------------------------------------------
    /// <summary>
    /// Resource.Packを保持し、検索、取得できる機能を提供します.
    /// </summary>
    //----------------------------------------------------------------------------------------------
    public class Cache
    {
        //=== Variables
        private Dictionary<string, Pack> container = null;

        //=== Initialization

        public static Cache Create()
        {
            return new Cache();
        }

        private Cache()
        {
            this.container = new Dictionary<string, Pack>();
        }

        //=== Control

        /// <summary>
        /// Pack.GetId()をKeyとして追加します
        /// </summary>
        public void Register( Pack pack )
        {
            string id = pack.GetId();
            this.GetContainer()[id] = pack;
        }

        /// <summary>
        /// 該当のIdを削除します
        /// </summary>
        public void Remove( string id )
        {
            this.GetContainer().Remove(id);
        }

        /// <summary>
        /// 全てRemove()します
        /// </summary>
        public void Clear()
        {
            this.GetContainer().Clear();
        }

        /// <summary>
        /// idに該当するものがなければ null を返します
        /// </summary>
        public Pack GetOrNull( string id )
        {
            Pack pack = null;

            if ( this.GetContainer().TryGetValue(id, out pack) == false )
            {
                return null;
            }

            return pack;
        }

        //=== Container

        /// <summary>
        /// this.containerへの参照を返します
        /// </summary>
        private Dictionary<string, Pack> GetContainer()
        {
            return this.container;
        }

        /// <summary>
        /// 保持しているものをListで返します
        /// </summary>
        public List<Pack> GetAll()
        {
            List<Pack>               list      = new List<Pack>();
            Dictionary<string, Pack> container = this.GetContainer();

            foreach ( KeyValuePair<string, Pack> pair in container )
            {
                list.Add(pair.Value);
            }

            return list;
        }
    }
}
#endif