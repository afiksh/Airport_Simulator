namespace DataBase.Repository
{
    public interface IRepository<TEntity>
    {
        #region Get
        /// <summary>
        /// Get all <typeparamref name="TEntity"/> in the data base
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity>? GetAll();

        /// <summary>
        /// Get all <typeparamref name="TEntity"/> in the coming to/at the airport
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity>? GetAllActive();

        /// <summary>
        /// Get all <typeparamref name="TEntity"/> in the air
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAllInAir();

        /// <summary>
        /// Get <typeparamref name="TEntity"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> GetById(int id);
        #endregion

        #region Post
        /// <summary>
        /// Add <paramref name="entity"/> to the data base
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity?> Post(TEntity entity);
        #endregion

        #region Delete
        /// <summary>
        /// Delete from the airport (not data base!!!) an <typeparamref name="TEntity"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> Delete(int id);

        /// <summary>
        /// Remove the first <paramref name="entity"/> from air queue
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity?> DeleteFirstInAir(TEntity entity);
        #endregion

        #region Put
        /// <summary>
        /// Update <typeparamref name="TEntity"/> (by <paramref name="id"/>) leg to the next one
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity?> UpdateLeg(int id);
        #endregion
    }
}