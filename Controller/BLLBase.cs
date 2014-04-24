using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace Controller
{
    /// <summary>
    /// BLL基础类（所有BLL类的基类）
    /// </summary>
    /// <typeparam name="T">对应的Model类</typeparam>
    /// <typeparam name="K">对应的DAL类</typeparam>
    public class BLLBase<T, K>
        where T : new()
        where K : DALBase<T>, new()
    {
        /// <summary>
        /// Model类
        /// </summary>
        private Type _modeltype;

        /// <summary>
        /// Dal对象
        /// </summary>
        protected K _dal;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BLLBase()
        {
            _modeltype = typeof(T);
            _dal = new K();
        }

        /// <summary>
        /// 获得最大的ID
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxID()
        {
            return _dal.GetMaxID();
        }

        /// <summary>
        /// 查询数据库函数
        /// </summary>
        /// <param name="SQLStr">查询字符串</param>
        /// <returns>列表对象</returns>
        public virtual List<T> Query(string SQLStr)
        {
            return DataSetToList(_dal.Query(SQLStr));
        }

        /// <summary>
        /// 满足该条件的记录的数量
        /// </summary>
        /// <param name="where">条件字符串</param>
        /// <returns>存在的条数</returns>
        public virtual int Exists(string where)
        {
            return _dal.Exists(where);
        }

        /// <summary>
        /// 增加一个记录(返回新增记录的ID)
        /// </summary>
        /// <param name="model">新增记录对象</param>
        /// <returns>新增记录的ID</returns>
        public virtual int Add(T model)
        {
            return _dal.Add(model);
        }

        /// <summary>
        /// 更新一个记录
        /// </summary>
        /// <param name="model">更新记录的对象（需要有主键和需修改的值）</param>
        public virtual bool Set(T model)
        {
            return _dal.Set(model);
        }

        /// <summary>
        /// 删除一个记录
        /// </summary>
        /// <param name="model">被删除的记录对象（只需要有主键）</param>
        public virtual bool Delete(T model)
        {
            return _dal.Delete(model);
        }

        /// <summary>
        /// 根据条件删除某项
        /// </summary>
        /// <param name="where">条件字符串</param>
        public virtual int DeleteByWhere(string where)
        {
            return _dal.DeleteByWhere(where);
        }

        /// <summary>
        /// 获得某一列内容根据ID
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>结果对象</returns>
        public virtual T GetRow(int id)
        {
            List<T> list = DataSetToList(_dal.GetRow(id));
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获得所有记录
        /// </summary>
        /// <returns>结果队列</returns>
        public virtual List<T> GetAll()
        {
            return DataSetToList(_dal.GetAll());
        }

        /// <summary>
        /// 获得所有满足条件的记录
        /// </summary>
        /// <param name="where">查询字符串</param>
        /// <returns>结果队列</returns>
        public virtual List<T> GetByWhere(string where)
        {
            return DataSetToList(_dal.GetByWhere(where));
        }

        /// <summary>
        /// 在不同的排序中获得前几条记录
        /// </summary>
        /// <param name="top">前几条</param>
        /// <param name="descTag">true : 由大到小, false : 由小到大 </param>
        /// <returns>结果队列</returns>
        public virtual List<T> GetTop(int top, bool descTag)
        {
            return DataSetToList(_dal.GetTop(top, descTag));
        }

        /// <summary>
        /// 根据不同的条件和顺序获取某段记录
        /// </summary>
        /// <param name="RowIndex">开始位置</param>
        /// <param name="Count">读取数量</param>
        /// <param name="where">条件</param>
        /// <param name="descTag">true : 由大到小, false : 由小到大</param>
        /// <param name="RowCount">总数据量</param>
        /// <returns>结果队列</returns>
        public virtual List<T> GetRangeByWhere(int RowIndex, int Count, string where, bool descTag, out int RowCount)
        {
            return this.GetRangeByWhere(RowIndex, Count, where, descTag, null, out RowCount);
        }

        /// <summary>
        /// 根据条件和排序查询某段记录
        /// </summary>
        /// <param name="RowIndex">起始行</param>
        /// <param name="Count">查询数量</param>
        /// <param name="where">条件字符串</param>
        /// <param name="descTag">是否为倒序（false：正序，true：倒序）</param>
        /// <param name="descStr">需要被排序的列名</param>
        /// <param name="RowCount">能被查询到的数量</param>
        /// <returns>结果队列</returns>
        public virtual List<T> GetRangeByWhere(int RowIndex, int Count, string where, bool descTag, string descStr, out int RowCount)
        {
            return DataSetToList(_dal.GetRangeByWhere(RowIndex, Count, where, descTag, descStr, out RowCount));
        }

        /// <summary>
        /// DataSet转化为List
        /// </summary>
        /// <param name="ds">被转化的DataSet</param>
        /// <returns>结果队列</returns>
        public List<T> DataSetToList(DataSet ds)
        {
            List<T> ResultList = new List<T>();
            if (ds.Tables.Count > 0)
            {
                DataTableReader dtr = new DataTableReader(ds.Tables[0]);
                T tempModel;
                while (dtr.Read())
                {
                    tempModel = new T();
                    MemberInfo[] miArray = _modeltype.GetMembers();
                    foreach (MemberInfo mi in miArray)
                    {
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            try
                            {
                                if (dtr[mi.Name] == DBNull.Value)
                                {
                                    continue;
                                }
                                _modeltype.InvokeMember(mi.Name, BindingFlags.SetProperty, null, tempModel, new object[] { dtr[mi.Name] });
                            }
                            catch (Exception ex)
                            {
                                string EStr = string.Format("Error:{0} {1}", ex.StackTrace, ex.Message);
                                Console.WriteLine(EStr);
                            }
                        }
                    }
                    ResultList.Add(tempModel);
                }
                dtr.Close();
            }
            return ResultList;
        }
    }
}
