using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using System.Reflection;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

using App.Data;
using App.Data.Model.Users;
using App.Exceptions.Auth;
using App.Service.Auth;

namespace App.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(AppDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await GetUserById(id, false);
        }

        public async Task<User> GetUserById(Guid id, bool includeInactive)
        {
            if (includeInactive)
                return await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id.Equals(id));
            else
                return await _context.Users.FindAsync(id);
        }

        public async Task<IList<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.PlayStyle)
                .OrderBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsers(IDictionary<string, string> queryParams)
        {
            IQueryable<User> result = null;

            if (QueryableUtil.IncludeDeleted(queryParams))
            {
                result = _context.Users
                .Include(u => u.PlayStyle)
                .IgnoreQueryFilters()
                .AsQueryable();
            }
            else
            {
                result = _context.Users
               .Include(u => u.PlayStyle)
               .AsQueryable();
            }

            Type entityType = typeof(User);

            // Ensure the name (`u`) for the ParameterExpression matches what was given for the IQueryable (e.g. _context.Users.Include(u ...)
            ParameterExpression parameterExpression = Expression.Parameter(entityType, "u");
            Expression where = QueryableUtil.MakeWhereClause(entityType, parameterExpression, queryParams);
            if (where != null)
            {
                result = result.Where(Expression.Lambda<Func<User, bool>>(where, parameterExpression));
            }

            // QueryableUtil.MakeWhereClause();
            /*             _context.Users
                            .Include(u => u.DominantHand)
                            .IgnoreQueryFilters()
                            .Where(u => u.Email == "daniil.medvedev@gmail.com"); */

            return await result.ToListAsync();

            /*           // IQueryable<User> result = null;
                      IQueryable<User> result = _context.Users
                          .Include(u => u.DominantHand);

                      Type entityType = typeof(User);
                      ParameterExpression parameterExpression = Expression.Parameter(entityType, "e"); // e =>
                      Expression where = QueryableUtil.MakeWhereClause(result, entityType, Expression.Parameter(entityType, "e"), queryParams);
                      if (where != null)
                      {
                          result = result.Where(Expression.Lambda<Func<User, bool>>(where, parameterExpression));
                          // result.Where(where);
                      }

                      QueryableUtil.IgnoreQueryFilters(result, queryParams); */

            // IgnoreQueryFilters(result, queryParams);
            // MakeWhereClause(result, queryParams);

            // result.ToString();
            // return await result.ToListAsync();
        }

        /*         public void IgnoreQueryFilters(IQueryable<User> result, IDictionary<string, string> queryParams)
                {
                    if (queryParams.ContainsKey("IncludeInactive") && queryParams["IncludeInactive"].Equals("true"))
                    {
                        result
                        .IgnoreQueryFilters();
                    }
                } */
        public async Task<User> Create(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User is required");
            }

            if (!await IsEmailAvailable(user.Email))
            {
                throw new ArgumentException(String.Format("O endereço de email [{0}] não está disponivel.", user.Email));
            }

            user.Id = Guid.NewGuid();
            if (!String.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = _passwordHasher.HashPassword(user.Password);
            }

            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<User> Update(Guid id, User user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            cUser.Updated = DateTime.Now;
            cUser.LastName = user.LastName;

            _context.Users.Update(cUser);
            await _context.SaveChangesAsync();

            return cUser;
        }

        public async Task UpdatePartial(Guid id, JsonPatchDocument<User> user)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            user.ApplyTo(cUser);
            cUser.Updated = DateTime.Now;
            await _context.SaveChangesAsync();

        }

        public async Task Delete(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("The user provided does not contain an Id");
            }

            User cUser = await GetUserById(id);

            if (cUser == null)
            {
                throw new ApplicationException(String.Format("User not found for Id {0}", id));
            }

            cUser.Updated = DateTime.Now;
            cUser.IsDeleted = true;

            _context.Users.Update(cUser);
            await _context.SaveChangesAsync();

        }

        public void checkEmailAddress(String email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("The email cannot be null or empty");
            }

            if (!isValidEmail(email))
            {
                throw new ArgumentException(String.Format("The email [{0}] is not valid", email));
            }

        }

        public bool isValidEmail(String email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        public async Task<bool> IsEmailAvailable(String email)
        {
            checkEmailAddress(email);

            User user = await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            return user == null;
        }

        public async Task<User> SignInWithFacebook(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("O usuário é obrigatório");
            }

            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.FacebookId))
            {
                throw new ArgumentException("Os campos Email e FacebookId são obrigatórios.");
            }

            User cUser = await _context.Users
                .Where(u =>
                    u.Email == user.Email &&
                    u.FacebookId == user.FacebookId)
                .FirstAsync();

            if (cUser != null)
            {
                cUser.LastLogin = DateTime.Now;

                _context.Users.Update(cUser);
                await _context.SaveChangesAsync();
            }

            return cUser;
        }

        public async Task<User> SignInWithBasicAuth(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("O usuário é obrigatório");
            }

            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Os campos Email e Senha são obrigatórios.");
            }

            User cUser = await _context.Users
                .Where(u =>
                    u.Email == user.Email)
                .FirstAsync();

            if (cUser == null ||
                (cUser != null && _passwordHasher.VerifyHashedPassword(cUser.Password, user.Password).Equals(PasswordVerificationResult.Failed)))
            {
                throw new BasicAuthenticationException();
            }

            cUser.LastLogin = DateTime.Now;
            _context.Users.Update(cUser);
            await _context.SaveChangesAsync();

            return cUser;
        }
        private void MakeWhereClause(IQueryable<User> result, IDictionary<string, string> queryParams)
        {

            Expression where = null;

            Type entityType = typeof(User);
            ParameterExpression parameterExpression = Expression.Parameter(entityType, "e"); // e =>

            foreach (KeyValuePair<string, string> kvp in queryParams)
            {
                if (kvp.Key != "sort" && kvp.Key != "IncludeInactive")
                {
                    if (where == null)
                    {
                        where = MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value);
                    }
                    else
                    {
                        where = Expression.And(where, MakePropertyClause(entityType, parameterExpression, kvp.Key, kvp.Value));
                    }
                }
            }

            if (where != null && result == null)
            {
                result = _context.Users
                    .Where(Expression.Lambda<Func<User, bool>>(where, parameterExpression));
            }
        }

        private Expression MakePropertyClause(Type entityType, ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            PropertyInfo propertyInfo = entityType.GetProperty(memberName);

            if (propertyInfo == null)
            {
                throw new ArgumentException(String.Format("O campo {0} não existe.", memberName));
            }

            Expression expression = null;

            switch (propertyInfo.PropertyType.Name)
            {
                case "Int32":
                    expression = MakePropertyIntClause(parameterExpression, memberName, memberValue);
                    break;

                // String
                default:
                    expression = MakePropertyStringClause(parameterExpression, memberName, memberValue);
                    break;
            }
            return expression;
        }

        private Expression MakePropertyStringClause(ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            Expression fieldProperty = Expression.Property(parameterExpression, memberName);
            Expression fieldClause = Expression.Equal(fieldProperty, Expression.Constant(memberValue));

            return Expression.Equal(fieldProperty, Expression.Constant(memberValue));
            // return Expression.Lambda<Func<User, bool>>(fieldClause, parameterExpression);
            // return Expression.Equal(fieldProperty, Expression.Constant(memberValue));
        }

        // private Expression<Func<User, bool>> MakePropertyIntClause(Type entityType, string memberName, string memberValue)
        private Expression MakePropertyIntClause(ParameterExpression parameterExpression, string memberName, string memberValue)
        {
            Expression fieldProperty = Expression.Property(parameterExpression, memberName);
            // Expression fieldClause = Expression.Equal(fieldProperty, Expression.Constant(Convert.ToInt32(memberValue), typeof(int)));
            return Expression.Equal(fieldProperty, Expression.Constant(Convert.ToInt32(memberValue), typeof(int)));
            // return Expression.Equal(fieldProperty, Expression.Constant(memberValue));
        }

        private Expression<Func<User, bool>> MakePropertyStringClauseB(IDictionary<string, string> queryParams)
        {
            // Time to build up the clause in the ANY field
            ParameterExpression parameterExpression = Expression.Parameter(typeof(User), "u"); // u =>

            Expression where = null;

            foreach (KeyValuePair<string, string> kvp in queryParams)
            {
                if (kvp.Key != "sort")
                {
                    Expression fieldProperty = Expression.Property(parameterExpression, kvp.Key);
                    Expression fieldClause = Expression.Equal(fieldProperty, Expression.Constant(kvp.Value));

                    if (where == null)
                    {
                        where = fieldClause;
                    }
                    else
                    {
                        where = Expression.And(where, fieldClause);
                    }
                }
            }

            return Expression.Lambda<Func<User, bool>>(where, parameterExpression);
        }
    }
}