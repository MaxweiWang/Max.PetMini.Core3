using Max.PetMini.DAL.Models;
using Max.PetMini.Extension;
using System;
using System.Reflection;

namespace Max.PetMini.DAL.Extensions
{
    /// <summary>
    /// 模型拓展
    /// </summary>
    public static class ViewModelExtensions
    {
        /// <summary>
        /// 转换DAL视图模型到BLL视图模型
        /// </summary>
        /// <typeparam name="DALViewModel"></typeparam>
        /// <typeparam name="BLLViewModel"></typeparam>
        /// <param name="dalViewModel"></param>
        /// <param name="bllViewModel"></param>
        /// <returns></returns>
        public static BLLViewModel ToViewModel<DALViewModel, BLLViewModel>(this DALViewModel dalViewModel, out BLLViewModel bllViewModel) where BLLViewModel : DALViewModel, new() where DALViewModel : ViewModel, new()
        {
            var outputBLLViewModel = new BLLViewModel();
            //子类类型
            var childType = outputBLLViewModel.GetType();
            //查询所有父类属性
            foreach (var parentPropertiy in dalViewModel.GetType().GetProperties())
            {
                //获取到子类相同的属性
                var childPropertity = childType.GetProperty(parentPropertiy.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                //如果子类没有与父类相同的属性名,则搜索父类
                if (childPropertity == null)
                {
                    childPropertity = childType.GetProperty(parentPropertiy.Name);
                }
                //如果子类属性不可写或者父类属性不可读
                if (!childPropertity.CanWrite || !parentPropertiy.CanRead)
                {
                    continue;
                }

                //如果子类是枚举类型
                if (childPropertity.PropertyType.IsEnum)
                {
                    //将父类值赋给子类
                    try
                    {
                        //自定义枚举类型的转换(ToPascalCase)
                        childPropertity.SetValue(outputBLLViewModel, Enum.Parse(childPropertity.PropertyType, parentPropertiy.GetValue(dalViewModel).ToString().ToPascalCase()));
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    continue;
                }
                //将父类值赋给子类
                childPropertity.SetValue(outputBLLViewModel, parentPropertiy.GetValue(dalViewModel));
            }
            bllViewModel = outputBLLViewModel;
            return outputBLLViewModel;
        }
    }
}
