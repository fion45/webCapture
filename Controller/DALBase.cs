using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using DBUnit;

namespace Controller
{
    /// <summary>
    /// DAL基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DALBase<T>
        where T : new()
    {
        /// <summary>
        /// 模型的类型
        /// </summary>
        public Type _modeltype;

        /// <summary>
        /// 相对应的DAL类
        /// </summary>
        private Type _dalclass;

        /// <summary>
        /// 表格名称
        /// </summary>
        private string _tablename;

        /// <summary>
        /// 主键名称
        /// </summary>
        private string _key;

        /// <summary>
        /// 主键是否是自增长的种子
        /// </summary>
        private bool _keyisidentity;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DALBase()
        {
            try
            {
                _modeltype = typeof(T);
                _dalclass = this.GetType();
                string tempStr = _modeltype.Name;
                //表名
                _tablename = "tb_" + tempStr;
                //从数据库里获取该表的Key
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@TableName",SqlDbType.VarChar,50)
                };
                parameters[0].Value = _tablename;
                DataSet ds = DBHelperSQL.RunProcedure("sp_common_GetTableKey", parameters, "ds");
                DataTableReader dtr = new DataTableReader(ds.Tables[0]);
                if (dtr.Read())
                {
                    _key = dtr["key"].ToString();
                    _keyisidentity = ((int)dtr["isIdentity"] == 1) ? true : false;
                }
                else
                {
                    _key = null;
                }
            }
            catch
            {
                //创建DALBase失败
            }
        }

        #region 每个表公有的接口
        /// <summary>
        /// 获得主键最大的值
        /// </summary>
        /// <returns>Max ID</returns>
        public int GetMaxID()
        {
            if (_key.Length > 0)
            {
                return DBHelperSQL.GetMaxID(_key, _tablename);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 执行查询该表格的SQL语句
        /// </summary>
        /// <param name="SQLStr">SQL语句</param>
        /// <returns>查询到的dataset</returns>
        public DataSet Query(string SQLStr)
        {
            return DBHelperSQL.Query(SQLStr);
        }

        public int Exists(string where)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@Where", SqlDbType.VarChar,500)
            };
            parameters[0].Value = where;
            int rowsAffected;
            return DBHelperSQL.RunProcedure("sp_" + _tablename + "_Exists", parameters, out rowsAffected);
        }

        /// <summary>
        /// 向表中插入数据
        /// </summary>
        /// <param name="obj">添加的对象模型</param>
        /// <returns>返回最新的ID</returns>
        public int Add(T obj)
        {
            SqlParameter[] parameters = CreateParameters(obj, true, false, true, null);
            int rowsAffected;
            return DBHelperSQL.RunProcedure("sp_" + _tablename + "_Insert", parameters, out rowsAffected);
        }

        /// <summary>
        /// 不存在KEY的数据表不能调用
        /// </summary>
        /// <param name="obj">该类型的对象</param>
        public bool Set(T obj)
        {
            if (!string.IsNullOrEmpty(_key))
            {
                SqlParameter[] parameters = CreateParameters(obj, true, true, true, null);
                int rowAffect = 0;
                DBHelperSQL.RunProcedure("sp_" + _tablename + "_Update", parameters, out rowAffect);
                return rowAffect > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 不存在KEY的数据表不能调用
        /// </summary>
        /// <param name="obj">该类型的对象（必须含有KEYID）</param>
        public bool Delete(T obj)
        {
            if (!string.IsNullOrEmpty(_key))
            {
                SqlParameter[] parameters = CreateParameters(obj, true, true, false, null);
                int rowAffect = 0;
                DBHelperSQL.RunProcedure("sp_" + _tablename + "_DeleteRow", parameters, out rowAffect);
                return rowAffect > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得特定的项
        /// </summary>
        /// <param name="id">被查找的KEY ID</param>
        /// <returns>数据</returns>
        public DataSet GetRow(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@" + _key,id)
            };
            return DBHelperSQL.RunProcedure("sp_" + _tablename + "_SelectRow", parameters, "ds");
        }

        /// <summary>
        /// 获得所有项
        /// </summary>
        /// <returns>数据</returns>
        public DataSet GetAll()
        {
            return DBHelperSQL.RunProcedure("sp_" + _tablename + "_SelectAll", new SqlParameter[] { }, "ds");
        }

        /// <summary>
        /// 获得在特定一条件下的数据
        /// </summary>
        /// <param name="where">条件字符串</param>
        /// <returns>数据</returns>
        public DataSet GetByWhere(string where)
        {
            string SQLStr = "SELECT * FROM " + _tablename;
            if (!string.IsNullOrEmpty(where))
            {
                SQLStr += " WHERE " + where;
            }
            return DBHelperSQL.Query(SQLStr);
            //where 太长不好控制
            //SqlParameter[] parameters = new SqlParameter[]
            //{
            //    new SqlParameter("@Where",SqlDbType.VarChar,200)
            //};
            //parameters[0].Value = where;
            //return DBHelperSQL.RunProcedure("sp_" + _tablename + "_SelectByWhere", parameters, "ds");
        }

        /// <summary>
        /// 获得前几个结果
        /// </summary>
        /// <param name="top">前几个</param>
        /// <param name="descTag">是否倒序</param>
        /// <returns>数据</returns>
        public DataSet GetTop(int top, bool descTag)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Top",SqlDbType.Int,4),
                new SqlParameter("@DescTag",SqlDbType.Bit,1)
            };
            parameters[0].Value = top;
            parameters[1].Value = descTag;
            return DBHelperSQL.RunProcedure("sp_" + _tablename + "_SelectTop", parameters, "ds");
        }

        /// <summary>
        /// 根据条件，位置，数量，和排序获得数据（主要用于GridView里的数据获取，数据库分页技术）
        /// </summary>
        /// <param name="RowIndex">第几行</param>
        /// <param name="Count">多少个数据</param>
        /// <param name="where">条件字符串</param>
        /// <param name="descTag">是否倒序</param>
        /// <param name="descStr">排序的列名</param>
        /// <param name="RowCount">能查到的数据数量</param>
        /// <returns>结果</returns>
        public DataSet GetRangeByWhere(int RowIndex, int Count, string where, bool descTag, string descStr, out int RowCount)
        {
            SqlParameter[] parameters;
            if (!string.IsNullOrEmpty(descStr))
            {
                parameters = new SqlParameter[] {
                    new SqlParameter("@RowIndex",SqlDbType.Int,4),
                    new SqlParameter("@Count",SqlDbType.Int,4),
                    new SqlParameter("@Where",SqlDbType.VarChar,200),
                    new SqlParameter("@DescTag",SqlDbType.Bit,1),
                    new SqlParameter("@RowCount",SqlDbType.Int,4),
                    new SqlParameter("@DescStr",SqlDbType.VarChar,50)
                };
                parameters[0].Value = RowIndex;
                parameters[1].Value = Count;
                parameters[2].Value = where;
                parameters[3].Value = descTag;
                parameters[4].Direction = ParameterDirection.Output;
                parameters[5].Value = descStr;
            }
            else
            {
                parameters = new SqlParameter[] {
                    new SqlParameter("@RowIndex",SqlDbType.Int,4),
                    new SqlParameter("@Count",SqlDbType.Int,4),
                    new SqlParameter("@Where",SqlDbType.VarChar,200),
                    new SqlParameter("@DescTag",SqlDbType.Bit,1),
                    new SqlParameter("@RowCount",SqlDbType.Int,4)
                };
                parameters[0].Value = RowIndex;
                parameters[1].Value = Count;
                parameters[2].Value = where;
                parameters[3].Value = descTag;
                parameters[4].Direction = ParameterDirection.Output;
            }
            DataSet ds = DBHelperSQL.RunProcedure("sp_" + _tablename + "_SelectRangeByWhere", parameters, "ds");
            RowCount = int.Parse(parameters[4].Value.ToString());
            return ds;
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="where">条件字符串</param>
        /// <returns>成功删除的条数</returns>
        public int DeleteByWhere(string where)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Where",SqlDbType.VarChar,500)
            };
            parameters[0].Value = where;
            int rowAffect = 0;
            DBHelperSQL.RunProcedure("sp_" + _tablename + "_DeleteByWhere", parameters, out rowAffect);
            return rowAffect;
        }

        /// <summary>
        /// 创建存储过程的参数
        /// </summary>
        /// <param name="obj">模型对象</param>
        /// <param name="key">是否包含KEY</param>
        /// <param name="haveIdentity">是否需要唯一键</param>
        /// <param name="member">是否包含成员对象</param>
        /// <param name="sqlParams">需要包含的参数列表</param>
        /// <returns>生成的参数</returns>
        private SqlParameter[] CreateParameters(T obj, bool key, bool haveIdentity, bool member, SqlParameter[] sqlParams)
        {

            List<SqlParameter> tempList = new List<SqlParameter>();
            try
            {
                foreach (PropertyInfo pi in _modeltype.GetProperties())
                {
                    if (pi.Name == _key)
                    {
                        if (key)
                        {
                            if (!haveIdentity && _keyisidentity)
                            {
                                continue;
                            }
                            tempList.Add(new SqlParameter("@" + pi.Name,
                             _modeltype.InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null)));
                        }
                    }
                    else
                    {
                        if (member)
                        {
                            tempList.Add(new SqlParameter("@" + pi.Name,
                                _modeltype.InvokeMember(pi.Name, BindingFlags.GetProperty, null, obj, null)));
                        }
                    }
                }
                foreach (SqlParameter par in sqlParams)
                {
                    tempList.Add(par);
                }
            }
            catch
            {
                //构造Parameters失败
            }
            return tempList.ToArray();
        }

        #endregion


    }


}
