package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.isVisible
import androidx.fragment.app.viewModels
import androidx.lifecycle.Observer
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.paging.LoadState
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentHomepageBinding
import com.example.gurmedefteri.databinding.FragmentScoredFoodBinding
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import com.example.gurmedefteri.ui.adapters.ScoredFoodsAdapter
import com.example.gurmedefteri.ui.viewmodels.HomepageViewModel
import com.example.gurmedefteri.ui.viewmodels.ScoredFoodViewModel
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject

@AndroidEntryPoint
class ScoredFoodFragment : Fragment() {
    private lateinit var binding: FragmentScoredFoodBinding
    private lateinit var viewModel: ScoredFoodViewModel
    @Inject
    lateinit var scoredFoodsAdapter: ScoredFoodsAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentScoredFoodBinding.inflate(inflater, container, false)


        return binding.root
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {

        super.onViewCreated(view, savedInstanceState)
        binding.apply {

            lifecycleScope.launchWhenCreated {
                viewModel.foodsList.collect {
                    scoredFoodsAdapter.submitData(it)
                }
            }

            scoredFoodsAdapter.setOnItemClickListener {
                val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToFoodsDetailFragment(it)
                findNavController().navigate(direction)
            }

            lifecycleScope.launchWhenCreated {
                scoredFoodsAdapter.loadStateFlow.collect{
                    val state = it.refresh
                    prgBarMoviesScoredFoods.isVisible = state is LoadState.Loading
                }
            }


            scoredFoods.apply {
                layoutManager = LinearLayoutManager(requireContext())
                layoutManager = StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.VERTICAL)
                adapter = scoredFoodsAdapter
            }

            scoredFoods.adapter = scoredFoodsAdapter.withLoadStateFooter(
                LoadMoreAdapter{
                    scoredFoodsAdapter.retry()
                }
            )

        }
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val tempViewModel: ScoredFoodViewModel by viewModels()

        viewModel = tempViewModel

    }

    override fun onResume() {
        super.onResume()
    }


}