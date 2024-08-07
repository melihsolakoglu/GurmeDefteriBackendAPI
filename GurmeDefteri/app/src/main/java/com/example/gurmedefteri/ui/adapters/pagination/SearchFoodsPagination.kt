package com.example.gurmedefteri.ui.adapters.pagination

import android.util.Log
import androidx.paging.PagingSource
import androidx.paging.PagingState
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.repository.ApiServicesRepository
import retrofit2.HttpException
import retrofit2.http.Query

class SearchFoodsPagination(private val repository: ApiServicesRepository, private var query: String) : PagingSource<Int, Food>() {
    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, Food> {
        return try {
            val currentPage = params.key ?: 1
            val response = repository.getFoodSearchByPage(query,currentPage, 6)
            val data = response.body() ?: return LoadResult.Error(Exception("Response body is null"))
            if (data.isEmpty()) {
                return LoadResult.Page(
                    data = emptyList(),
                    prevKey = if (currentPage == 1) null else currentPage - 1,
                    nextKey = null
                )
            } else {
                LoadResult.Page(
                    data = data,
                    prevKey = if (currentPage == 1) null else currentPage - 1,
                    nextKey = currentPage + 1
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