package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.isVisible
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentMoreFoodsBinding
import com.example.gurmedefteri.ui.adapters.MoreFoodsAdapter
import com.example.gurmedefteri.ui.viewmodels.MoreFoodsViewModel
import dagger.hilt.android.AndroidEntryPoint
import androidx.navigation.fragment.findNavController
import androidx.navigation.fragment.navArgs
import androidx.paging.LoadState
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import kotlinx.coroutines.launch
import javax.inject.Inject

@AndroidEntryPoint
class MoreFoodsFragment : Fragment() {
    private lateinit var binding: FragmentMoreFoodsBinding
    private lateinit var viewModel: MoreFoodsViewModel
    @Inject
    lateinit var scoredFoodsAdapter: MoreFoodsAdapter
    private  lateinit var QueryKey:String

    private var controlMain = false

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentMoreFoodsBinding.inflate(inflater, container, false)


        val swipeRefreslLayout = binding.swipeRefreshLayout
        swipeRefreslLayout.setOnRefreshListener {
            swipeRefreslLayout.isRefreshing = false
            onTasksNotCompleted()


        }

        val parentActivity = activity as? AppCompatActivity

        parentActivity?.supportActionBar?.apply {
            title = QueryKey
        }

        return binding.root
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {

        super.onViewCreated(view, savedInstanceState)
        onTasksNotCompleted()
    }

    fun createAdapter (){
        val mainFoodsAdapter = MoreFoodsAdapter()

        viewModel.changeLists()
        binding.apply {
            lifecycleScope.launch {
                viewModel.foodsList.collect {
                    mainFoodsAdapter.submitData(it)
                }
            }

        }

        mainFoodsAdapter.setOnItemClickListener {
            val direction = MoreFoodsFragmentDirections.actionMoreFoodsFragmentToFoodsDetailFragment(it)
            findNavController().navigate(direction)
        }


        lifecycleScope.launch {
            mainFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMoviesMoreFoods.isVisible = state is LoadState.Loading
            }
        }
        binding.moreFoods.apply {
            layoutManager = LinearLayoutManager(requireContext())
            layoutManager = StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.VERTICAL)
            adapter = mainFoodsAdapter
        }

        binding.moreFoods.adapter=mainFoodsAdapter.withLoadStateFooter(
            LoadMoreAdapter{
                mainFoodsAdapter.retry()
            }
        )

    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val bundle:MoreFoodsFragmentArgs by navArgs()
        val scoreUnscore = bundle.unscoreOrScore
        val queryKey = bundle.queryKey

        val tempViewModel: MoreFoodsViewModel by viewModels()
        viewModel = tempViewModel
        viewModel.scoreOrUnscoreViewModel.value = scoreUnscore
        viewModel.queryKeyViewModel.value = queryKey

        QueryKey =queryKey




    }

    override fun onResume() {
        super.onResume()
    }

    private fun onTasksNotCompleted(){
        controlMain = false
        createAdapter()
        viewModel.controlMainFoodsLists()
    }


}