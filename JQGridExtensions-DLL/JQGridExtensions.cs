using System;
using System.Text;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

/*Description: The purpose of this DLL is to perform Server Side JQGrid Operations
 * 1. Server Side Filter Tool Bar Search
 * 2. Server Side Sorting 
 * 3. Server Side Advanced Search
 * @Author: Pabitro Singh
 */

namespace JQGridExtensions
{
    public static class JQGridExtensions
    {

        #region :::Sorting Function:::
        /// <summary>
        /// Dynamically perform server side sorting 
        /// </summary>
        /// <typeparam name="T">Generic Type e.g. class type</typeparam>
        /// <param name="Data">IQueryable Records or collection</param>
        /// <param name="SortByField">sidx sorting filed</param>
        /// <param name="SortOrder">sorting order</param>
        /// <returns>Returns Generic IQueryable</returns>
        public static IQueryable<T> JQGridSorting<T>(this IQueryable<T> Data, string SortByField, string SortOrder)
        {
            MethodCallExpression exp = null;
            IQueryable<T> result = null;
            string method = (SortOrder == "asc" ? "OrderBy" : "OrderByDescending");

            if (SortByField != string.Empty && SortOrder != string.Empty)
            {
                ParameterExpression pe = Expression.Parameter(typeof(T), "item");

                Expression prop = Expression.Property(pe, SortByField);

                LambdaExpression le = Expression.Lambda(prop, pe);

                Type[] types = new Type[] { Data.ElementType, le.Body.Type };

                exp = Expression.Call(typeof(Queryable), method, types, Data.Expression, le);

                result = Data.Provider.CreateQuery<T>(exp);
            }
            else
            {
                throw new ArgumentException("Provide all required arguments");
            }

            return result;

        }
        #endregion

        #region:::Filter Tool Bar Search Function:::

        /// <summary>
        /// Dynamically perform server side JQGrid Filter Tool Bar Search 
        /// </summary>
        /// <typeparam name="T">Generic Type e.g. class type</typeparam>
        /// <param name="Data">IQueryable Records or collection</param>
        /// <param name="filters">JQGrid supplies this values, stringResult: true,searchOnEnter: true,searchOperators: true</param>
        /// <returns>Returns Generic IQueryable</returns>
        public static IQueryable<T> JQGridToolBarSearch<T>(this IQueryable<T> Data, JQGridFilters filters)
        {
            IQueryable<T> result = null;

            // e.g. item =>item
            ParameterExpression pe = Expression.Parameter(typeof(T), "item");

            //e.g. item=>item.Name
            MemberExpression me = null;

            //constant value e.g. item=>item.Name='Rahul'
            ConstantExpression ce = null;

            //expression which evalutes to True / False
            Expression exp = null;

            //expression with evalutes methods 
            //MethodCallExpression exp = null;

            //And , Or 
            string Condition = filters.groupOp;

            // JQGrid supported searchoptions / sopt
            Op Operator = new Op();

            int count = filters.rules.Count;


            for (int i = 0; i < count; i++)
            {
                Operator = filters.rules.ElementAt(i).op;

                switch (Operator)
                {
                    case Op.eq:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.Equal(me, ce);
                        break;
                    case Op.ne:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.NotEqual(me, ce);
                        break;
                    case Op.lt:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.LessThan(me, ce);
                        break;
                    case Op.le:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.LessThanOrEqual(me, ce);
                        break;
                    case Op.gt:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.GreaterThan(me, ce);
                        break;
                    case Op.ge:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        exp = Expression.GreaterThanOrEqual(me, ce);
                        break;
                    case Op.bw:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.BeginsWith(me, ce);
                        break;
                    case Op.bn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.NotBeginsWith(me, ce);
                        break;
                    case Op.In:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.In(me, ce);
                        break;
                    case Op.ni:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.NotIn(me, ce);
                        break;
                    case Op.ew:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.EndsWith(me, ce);
                        break;
                    case Op.en:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.NotEndsWith(me, ce);
                        break;
                    case Op.cn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.Contains(me, ce);
                        break;
                    case Op.nc:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.NotContains(me, ce);
                        break;
                    case Op.nu:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.Null(me, ce);
                        break;
                    case Op.nn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        exp = JQGridExtensions.NotNull(me, ce);
                        break;
                    default:
                        break;
                }

                //Compiling the lambda expression
                if (i == 0)
                {
                    if (exp != null)
                    {
                        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(exp, pe);
                        result = Data.AsQueryable().Where(lambda.Compile()).AsQueryable();
                    }
                }
                else
                {
                    if (exp != null)
                    {
                        Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(exp, pe);
                        result = result.AsQueryable().Where(lambda.Compile()).AsQueryable();
                    }
                }
            }

            return result;
        }

        #endregion


        #region:::Advanced Search Function:::

        /// <summary>
        /// Dynamically perform server side JQGrid Advanced Search 
        /// </summary>
        /// <typeparam name="T">Generic Type e.g. class type</typeparam>
        /// <param name="Data">IQueryable Records or collection</param>
        /// <param name="filters">JQGrid supplies this values, stringResult: true,searchOnEnter: true,searchOperators: true</param>
        /// <returns>Returns Generic IQueryable</returns>
        public static IQueryable<T> JQGridAdvancedSearch<T>(this IQueryable<T> Data, JQGridAdvanceFilter filters)
        {
            IQueryable<T> result = null;

            // e.g. item =>item
            ParameterExpression pe = Expression.Parameter(typeof(T), "item");

            //e.g. item=>item.Name
            MemberExpression me = null;

            //constant value e.g. item=>item.Name='Rahul'
            ConstantExpression ce = null;

            //expression which evalutes to True / False
            Expression exp = null;

            //Expression tree for Multiple Groups and Multiple conditions
            List<Expression> expTree = new List<Expression>();

            //expression with evalutes methods 
            //MethodCallExpression exp = null;

            //And , Or 
            string Condition = filters.groupOp;

            // JQGrid supported searchoptions / sopt
            Op Operator = new Op();

            int count = filters.rules.Count;


            for (int i = 0; i < count; i++)
            {
                Operator = filters.rules.ElementAt(i).op;
                switch (Operator)
                {
                    case Op.eq:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.Equal(me, ce));
                        break;
                    case Op.ne:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.NotEqual(me, ce));
                        break;
                    case Op.lt:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.LessThan(me, ce));
                        break;
                    case Op.le:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.LessThanOrEqual(me, ce));
                        break;
                    case Op.gt:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.GreaterThan(me, ce));
                        break;
                    case Op.ge:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data.ConvertToType(me.Type));
                        expTree.Add(Expression.GreaterThanOrEqual(me, ce));
                        break;
                    case Op.bw:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.BeginsWith(me, ce));
                        break;
                    case Op.bn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.NotBeginsWith(me, ce));
                        break;
                    case Op.In:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add((JQGridExtensions.In(me, ce)));
                        break;
                    case Op.ni:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add((JQGridExtensions.NotIn(me, ce)));
                        break;
                    case Op.ew:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.EndsWith(me, ce));
                        break;
                    case Op.en:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add((JQGridExtensions.NotEndsWith(me, ce)));
                        break;
                    case Op.cn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.Contains(me, ce));
                        break;
                    case Op.nc:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.NotContains(me, ce));
                        break;
                    case Op.nu:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.Null(me, ce));
                        break;
                    case Op.nn:
                        me = Expression.PropertyOrField(pe, filters.rules.ElementAt(i).field);
                        ce = Expression.Constant(filters.rules.ElementAt(i).data);
                        expTree.Add(JQGridExtensions.NotNull(me, ce));
                        break;
                    default:
                        break;
                }
            }

            //Anding 
            if (Condition.ToUpper() == "AND")
            {
                exp = expTree.Aggregate((exp1, exp2) => Expression.And(exp1, exp2));
               // expTree.Add(expTree.Aggregate((exp1, exp2) => Expression.And(exp1, exp2)));
            }

            // Oring
            if (Condition.ToUpper() == "OR")
            {
                exp = expTree.Aggregate((exp1, exp2) => Expression.Or(exp1, exp2));
                //expTree.Add(expTree.Aggregate((exp1, exp2) => Expression.Or(exp1, exp2)));
            }


            //emptying the list 
            expTree.Clear();

            //Multiple Groups
            for (int j = 0; j < filters.groups.Count; j++)
            {
                for (int k = 0; k < filters.groups[j].rules.Count; k++)
                {
                    Operator = filters.groups[j].rules.ElementAt(k).op;

                    switch (Operator)
                    {
                        case Op.eq:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.Equal(me, ce));
                            break;
                        case Op.ne:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.NotEqual(me, ce));
                            break;
                        case Op.lt:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.LessThan(me, ce));
                            break;
                        case Op.le:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.LessThanOrEqual(me, ce));
                            break;
                        case Op.gt:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.GreaterThan(me, ce));
                            break;
                        case Op.ge:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data.ConvertToType(me.Type));
                            expTree.Add(Expression.GreaterThanOrEqual(me, ce));
                            break;
                        case Op.bw:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.BeginsWith(me, ce));
                            break;
                        case Op.bn:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.NotBeginsWith(me, ce));
                            break;
                        case Op.In:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add((JQGridExtensions.In(me, ce)));
                            break;
                        case Op.ni:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add((JQGridExtensions.NotIn(me, ce)));
                            break;
                        case Op.ew:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.EndsWith(me, ce));
                            break;
                        case Op.en:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add((JQGridExtensions.NotEndsWith(me, ce)));
                            break;
                        case Op.cn:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.Contains(me, ce));
                            break;
                        case Op.nc:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.NotContains(me, ce));
                            break;
                        case Op.nu:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.Null(me, ce));
                            break;
                        case Op.nn:
                            me = Expression.PropertyOrField(pe, filters.groups[j].rules.ElementAt(k).field);
                            ce = Expression.Constant(filters.groups[j].rules.ElementAt(k).data);
                            expTree.Add(JQGridExtensions.NotNull(me, ce));
                            break;
                        default:
                            break;
                    }


                }


                Condition = filters.groups[j].groupOp.ToUpper();
                //Anding 
                if (Condition.ToUpper() == "AND")
                {
                    exp = expTree.Aggregate(exp,(exp1, exp2) => Expression.And(exp1, exp2));
                }

                // Oring
                if (Condition.ToUpper() == "OR")
                {
                    exp = expTree.Aggregate(exp,(exp1, exp2) => Expression.Or(exp1, exp2));
                }
            }


            //Compiling the lambda expression
            if (exp != null)
            {
                Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(exp, pe);
                result = Data.AsQueryable().Where(lambda.Compile()).AsQueryable();
            }
            return result;
        }

        #endregion

        #region:::Type Conversion Helper:::
        /// <summary>
        /// Convert to the filed value to T specific , where T is Generic class with properties  
        /// </summary>
        /// <param name="objct">Value to be converted</param>
        /// <param name="type">Type to which convertion needs to be done</param>
        /// <returns>it returns object</returns>
        public static object ConvertToType(this object objct, Type type)
        {
            TypeCode tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Boolean:
                    objct = Convert.ToBoolean(objct);
                    //objct = objct.ToString().ToLower() == "yes" ? true : false;
                    break;
                //case TypeCode.Byte:
                //    objct= Convert.ToByte(objct);
                //    break;
                case TypeCode.Char:
                    objct = Convert.ToChar(objct);
                    break;
                case TypeCode.DBNull:
                    objct = Convert.ChangeType(objct, TypeCode.DBNull);
                    break;
                case TypeCode.DateTime:
                    objct = Convert.ToDateTime(objct);
                    //objct = DateTime.Parse(objct.ToString());
                    break;
                case TypeCode.Decimal:
                    objct = Convert.ToDecimal(objct);
                    break;
                case TypeCode.Double:
                    objct = Convert.ToDouble(objct);
                    break;
                case TypeCode.Empty:
                    objct = Convert.ChangeType(objct, TypeCode.Empty);
                    break;
                case TypeCode.Object:
                    objct = Convert.ChangeType(objct, typeof(object));
                    break;
                case TypeCode.Int16:
                    objct = Convert.ToInt16(objct);
                    break;
                case TypeCode.Int32:
                    objct = Convert.ToInt32(objct);
                    break;
                case TypeCode.Int64:
                    objct = Convert.ToInt64(objct);
                    break;
                case TypeCode.SByte:
                    objct = Convert.ToSByte(objct);
                    break;
                case TypeCode.Single:
                    objct = Convert.ToSingle(objct);
                    break;
                case TypeCode.String:
                    objct = Convert.ToString(objct);
                    //    objct = objct.ToString().ToLower();
                    break;
                case TypeCode.UInt16:
                    objct = Convert.ToUInt16(objct);
                    break;
                case TypeCode.UInt32:
                    objct = Convert.ToUInt32(objct);
                    break;
                case TypeCode.UInt64:
                    objct = Convert.ToUInt64(objct);
                    break;
                default:
                    break;
            }

            return objct;
        }
        #endregion


        #region:::Helper Functions:::

        /// <summary>
        /// Starts With String operation, Gets invoked for 'bw' operation of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static MethodCallExpression BeginsWith(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            return Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce);

        }

        /// <summary>
        /// Does Not Starts With String operation, Gets invoked for 'bn' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static UnaryExpression NotBeginsWith(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            return Expression.IsFalse(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce));
        }


        /// <summary>
        /// Contains String operation, Gets invoked for 'in' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static MethodCallExpression In(MemberExpression me, ConstantExpression ce)
        {
            //IN is just the same function as CONTAINS  
            //so invoking the Contains function 
            return Contains(me, ce);
        }


        /// <summary>
        /// Does Not Contains String operation, Gets invoked for 'ni' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static UnaryExpression NotIn(MemberExpression me, ConstantExpression ce)
        {
            //NOT IN is just the same function as DOES NOT CONTAINS  
            //so invoking the NotContains function
            return NotContains(me, ce);
        }


        /// <summary>
        /// Ends With String operation, Gets invoked for 'ew' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static MethodCallExpression EndsWith(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
            return Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce);
        }


        /// <summary>
        ///Does Not Ends With String operation, Gets invoked for 'en' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static UnaryExpression NotEndsWith(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
            return Expression.IsFalse(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce));
        }


        /// <summary>
        /// Contains String operation, Gets invoked for 'in' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static MethodCallExpression Contains(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            return Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce);
             //----New code-----//
            //return Expression.Call(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), "ToUpper", Type.EmptyTypes), _method1, Expression.Call(Expression.Call(ce, "ToString", Type.EmptyTypes), "ToUpper", Type.EmptyTypes));
        }

        /// <summary>
        /// Does Not Contains String operation, Gets invoked for 'ni' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">Constant value to compare e.g R</param>
        /// <returns>Expression</returns>
        private static UnaryExpression NotContains(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            return Expression.IsFalse(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce));
            //----New code-----//
            //return Expression.Call(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), "ToUpper", Type.EmptyTypes), _method1, Expression.Call(Expression.Call(ce, "ToString", Type.EmptyTypes), "ToUpper", Type.EmptyTypes));

        }


        /// <summary>
        /// IsNullOrWhiteSpaceString operation, Gets invoked for 'nu' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">null value to compare</param>
        /// <returns>Expression</returns>
        private static MethodCallExpression Null(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });
            return Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce);
        }


        /// <summary>
        /// Not IsNullOrWhiteSpaceString operation, Gets invoked for 'nn' operator of JQGrid Sopt
        /// </summary>
        /// <param name="me">Property of field e.g. Name</param>
        /// <param name="ce">null value to compare</param>
        /// <returns>Expression</returns>
        private static UnaryExpression NotNull(MemberExpression me, ConstantExpression ce)
        {
            MethodInfo _method = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });
            return Expression.IsFalse(Expression.Call(Expression.Call(me, "ToString", Type.EmptyTypes), _method, ce));
        }

        public static string DecryptString(string str)
        {
            str = str.Replace("+", "%Plus%");
            str = HttpContext.Current.Server.UrlDecode(Uri.UnescapeDataString(str)).Replace("%lthash%", "&#");
            return str.Replace("%Plus%", "+");
        }
        #endregion
    }

    #region:::Helper Classes:::


    /// <summary>
    /// operations to be performed
    /// </summary>
    public enum Op
    {
        eq, // Equales==
        ne, // No Equals !=
        lt, // Less Than <
        le, // Less Than or Equals  <=
        gt, // Greater Than >
        ge, // Greater than or equals >=
        bw, // Starts With 
        bn, // Does not Start With 
        In, // In 
        ni, // Not In
        ew, // Ends With
        en, // Does not end with
        cn, // Contains
        nc, // Does not contains 
        nu, // null
        nn  // Not null

    }

    /// <summary>
    /// poco class for model binding the values posted by JQGrid
    /// </summary>
    public class JQGridSettings
    {
        public string filters { get; set; }
        public bool _search { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public string sord { get; set; }
        public string sidx { get; set; }
    }

    /// <summary>
    /// Poco class de-serialization filters json values posted by JQGrid 
    /// This calss is used for Tool Bar Search
    /// </summary>
    public class JQGridFilters
    {
        public JQGridFilters() { }
        public string groupOp { get; set; }
        public ICollection<Rules> rules { get; set; }
    }


    /// <summary>
    /// Poco class de-serialization filters json values posted by JQGrid 
    /// This calss is used for Advanced Search
    /// </summary>
    public class JQGridAdvanceFilter
    {
        public JQGridAdvanceFilter() { }
        public string groupOp { get; set; }
        public ICollection<Rules> rules { get; set; }
        public List<Groups> groups { get; set; }
        //public List<JQGridAdvanceFilter> groups { get; set; }


        //public string searchField { get; set; }
        //public string searchString { get; set; }
        //public string searchOper { get; set; }
    }

    /// <summary>
    /// This pococ class is used to data binding 
    /// filters values and operators posted by JQGrid
    /// </summary>
    public class Rules
    {
        public Rules() { }
        public string field { get; set; }
        public Op op { get; set; }
        public string data { get; set; }
    }

    public class Groups
    {
        public string groupOp { get; set; }
        public ICollection<Rules> rules { get; set; }
    }

    #endregion
}
