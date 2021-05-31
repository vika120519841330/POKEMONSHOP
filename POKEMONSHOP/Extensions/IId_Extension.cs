using POKEMONLIBRARY.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace POKEMONSHOP.Extensions
{
    public static class IId_Extension
    {
        /// <summary>
        /// Метод, осуществляющий проверку модели, которая наследует интерфейс IId на валидность
        /// </summary>
        /// <param name="model">Класс, который наследует интерфейс IId</param>
        /// <returns>Кортеж, содержащий признак валидности модели и коллекцию сообщений о конкретных ошибках валидности модели</returns>
        public static (bool, List<string>) CheckModelIsValid(this IId model)
        {
            bool result = true;

            List<string> errorsMessages = new List<string>();

            var validResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            var context = new ValidationContext(model);

            var modelIsValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, context, validResults, true);

            if (!modelIsValid)
            {
                foreach (var error in validResults)
                {
                    errorsMessages.Add(error.ErrorMessage);
                }
                result = false;
            }

            return (result, errorsMessages);
        }

        /// <summary>
        /// Метод для получения значений любых атрибутов у экземпляра заданного класса
        /// </summary>
        /// <typeparam name="T">Класс модели</typeparam>
        /// <typeparam name="TOut">Тип свойства, у которого считывается значение атрибута</typeparam>
        /// <typeparam name="TAttribute">Класс атрибута</typeparam>
        /// <typeparam name="TValue">Наименование свойства атрибута, чье значение считывается</typeparam>
        /// <param name="propertyExpression">Лямбда-выражение, относящееся к классу модели</param>
        /// <param name="valueSelector">>Лямбда-выражение, относящееся к классу атрибута</param>
        /// <returns>Строковое значение запрашиваемого атрибута</returns>
        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(Expression<Func<T, TOut>> propertyExpression,
                                                                                    Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var expression = (MemberExpression)propertyExpression.Body;

            var propertyInfo = (PropertyInfo)expression.Member;

            var attrib = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);

            var attri = attrib.FirstOrDefault();

            var attr = attri as TAttribute;

            return attr != null ? valueSelector(attr) : default(TValue);
        }

    }
}
