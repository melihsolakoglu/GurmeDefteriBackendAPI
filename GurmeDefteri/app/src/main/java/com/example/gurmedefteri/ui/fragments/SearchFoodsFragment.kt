package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.isVisible
import androidx.fragment.app.activityViewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.paging.LoadState
import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import androidx.paging.cachedIn
import androidx.paging.map
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.R
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.databinding.FragmentSearchFoodsBinding
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import com.example.gurmedefteri.ui.adapters.pagination.SearchFoodsPagination
import com.example.gurmedefteri.ui.viewmodels.SearchFoodsViewModel
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.launch
import javax.inject.Inject

@AndroidEntryPoint
class SearchFoodsFragment : Fragment() {
    private lateinit var binding: FragmentSearchFoodsBinding
    private val searchViewModel: SearchFoodsViewModel by activityViewModels()
    @Inject
    lateinit var mainFoodsAdapter: MainFoodsAdapter
    private var firstScreen =false
    private var firstQuery = true

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentSearchFoodsBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        setupRecyclerView()
        setupObservers()
        if(firstQuery){
            searchViewModel.query.value = "ÇokAnlamsızŞeylerSalladık"
            firstQuery =false
        }

    }


    private fun setupRecyclerView() {
        binding.SearchFoodsRV.apply {
            layoutManager = StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.VERTICAL)
            adapter = mainFoodsAdapter.withLoadStateFooter(
                footer = LoadMoreAdapter { mainFoodsAdapter.retry() }
            )
        }
    }

    private fun setupObservers() {
        searchViewModel.query.observe(viewLifecycleOwner) { newQuery ->
            val newAdapter = MainFoodsAdapter()
            lifecycleScope.launch {
                // RecyclerView temizleme işlemi


                // RecyclerView'un adapter'ını değiştir
                binding.SearchFoodsRV.adapter = newAdapter
                // Yeni verilerin yüklenmesi
                searchViewModel.foodsssList = Pager(PagingConfig(1)) {
                    SearchFoodsPagination(searchViewModel.krepo, newQuery)
                }.flow.cachedIn(lifecycleScope)

                setupLoadStateListener(newAdapter)
            }
            Log.d("SearchFoodsFragment", "Query: $newQuery")
            newAdapter.submitData(viewLifecycleOwner.lifecycle, PagingData.empty())
            viewLifecycleOwner.lifecycleScope.launch {
                searchViewModel.foodsssList.collectLatest { pagingData ->
                    Log.d("SearchFoodsFragment", "Paging data collected")


                    newAdapter.submitData(pagingData)
                }
            }

            newAdapter.setOnItemClickListener { food ->
                Log.d("SearchFoodsFragment", "Item clicked: $food")
                searchViewModel.isOnFoodSearch.value = true
                val direction = SearchFoodsFragmentDirections.actionSearchFoodsFragmentToFoodsDetailFragment(food)
                findNavController().navigate(direction)
            }

        }

    }

    private fun setupLoadStateListener(adapter: MainFoodsAdapter) {
        viewLifecycleOwner.lifecycleScope.launch {
            adapter.loadStateFlow.collectLatest { loadStates ->
                if (loadStates.refresh is LoadState.Loading) {
                    Log.d("SearchFoodsFragment", "Loading data")
                } else if (loadStates.refresh is LoadState.NotLoading) {
                    Log.d("SearchFoodsFragment", "Data loaded")
                }
                val itemCount = binding.SearchFoodsRV.adapter?.itemCount
                if(loadStates.refresh is LoadState.NotLoading){

                    if (itemCount == 0) {
                        if (searchViewModel.query.value == "ÇokAnlamsızŞeylerSalladık") {
                            binding.textView2.visibility = View.INVISIBLE
                        } else {
                            binding.textView2.visibility = View.VISIBLE
                        }
                    }
                    Log.d("SearchFoodsFragment", "Item count: $itemCount")
                    binding.prgBarMoviesSearchFoods.isVisible = false
                } else {
                    binding.textView2.visibility = View.INVISIBLE
                    binding.prgBarMoviesSearchFoods.isVisible = loadStates.refresh is LoadState.Loading
                    Log.d("SearchFoodsFragment", "Item count: $itemCount")
                }


            }
        }
    }
}