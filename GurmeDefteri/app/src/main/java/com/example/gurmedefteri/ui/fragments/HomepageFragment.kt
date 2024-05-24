package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.isVisible
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.NavHostFragment
import androidx.paging.LoadState
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentHomepageBinding
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import com.example.gurmedefteri.ui.viewmodels.HomepageViewModel
import dagger.hilt.android.AndroidEntryPoint
import javax.inject.Inject

@AndroidEntryPoint
class HomepageFragment : Fragment() {
    private lateinit var binding: FragmentHomepageBinding
    private lateinit var viewModel: HomepageViewModel
    @Inject
    lateinit var mainFoodsAdapter: MainFoodsAdapter

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentHomepageBinding.inflate(inflater,container,false)

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        binding.apply {

            lifecycleScope.launchWhenCreated {
                viewModel.foodsssList.collect {
                    mainFoodsAdapter.submitData(it)
                }
            }

            mainFoodsAdapter.setOnItemClickListener {
                val direction = HomepageFragmentDirections.actionHomepageToFoodsDetail(it)
                findNavController().navigate(direction)
            }

            lifecycleScope.launchWhenCreated {
                mainFoodsAdapter.loadStateFlow.collect{
                    val state = it.refresh
                    prgBarMovies.isVisible = state is LoadState.Loading
                }
            }


            mainFoods.apply {
                layoutManager = LinearLayoutManager(requireContext())
                layoutManager = StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.VERTICAL)
                adapter = mainFoodsAdapter
            }

            mainFoods.adapter=mainFoodsAdapter.withLoadStateFooter(
                LoadMoreAdapter{
                    mainFoodsAdapter.retry()
                }
            )

        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: HomepageViewModel by viewModels()
        viewModel = tempViewModel
    }

    override fun onResume() {
        super.onResume()
    }

}