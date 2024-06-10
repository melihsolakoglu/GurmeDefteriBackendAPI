package com.example.gurmedefteri.ui.adapters.pagination

import android.util.Log
import androidx.paging.PagingSource
import androidx.paging.PagingState
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import okhttp3.ResponseBody
import retrofit2.HttpException
import retrofit2.Response

class MoreFoodsSource(private val userId:String,
                      private val scoreOrUnscore:Int,
                      private val queryKey:String,
                      private val repository: ApiServicesRepository) : PagingSource<Int, Food>() {

    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, Food> {
        return try {
            val currentPage = params.key ?: 1
            var response: Response<List<Food>>

            if (scoreOrUnscore == 1){
                response = repository.getUnscoredCategorizeFoodsByPageWithUserId(userId,queryKey,currentPage,6)
            } else if(scoreOrUnscore == 2){
                response = repository.getScoredCategorizeFoodsByPageWithUserId(userId,queryKey,currentPage,6)
            }else{
                response = repository.getScoredFoodsByUserId(userId,currentPage, 6)

            }
            val data :List<Food>? = response.body()
            if(response.isSuccessful){
                if (data.isNullOrEmpty()) {
                    return LoadResult.Page(
                        data = emptyList(),
                        prevKey = if (currentPage == 1) null else currentPage - 1,
                        nextKey = null
                    )
                } else {
                    return LoadResult.Page(
                        data = data,
                        prevKey = if (currentPage == 1) null else currentPage - 1,
                        nextKey = currentPage + 1
                    )
                }
            }else{
                return LoadResult.Page(
                    data = emptyList(),
                    prevKey = if (currentPage == 1) null else currentPage - 1,
                    nextKey = null
                )
            }
        } catch (e: Exception) {
            LoadResult.Error(e)
        } catch (httpE: HttpException) {
            LoadResult.Error(httpE)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, Food>): Int? {
        return null
    }


}
