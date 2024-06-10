package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.isVisible
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.paging.LoadState
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.databinding.FragmentScoredFoodBinding
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import com.example.gurmedefteri.ui.viewmodels.ScoredFoodViewModel
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.launch

@AndroidEntryPoint
class ScoredFoodFragment : Fragment() {
    private lateinit var binding: FragmentScoredFoodBinding
    private lateinit var viewModel: ScoredFoodViewModel


    private var controlMain = false
    private var controlSoap = false
    private var controlDeserts = false
    private var controlDrinks = false

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentScoredFoodBinding.inflate(inflater, container, false)

        viewModel.isScoredAnyFood.observe(viewLifecycleOwner){
            if(it){
                binding.didntFoundScoredFood.visibility = View.INVISIBLE
            }else{
                binding.didntFoundScoredFood.visibility = View.VISIBLE
            }
        }

        val swipeRefreslLayout = binding.swipeRefreshLayout
        swipeRefreslLayout.setOnRefreshListener {
            swipeRefreslLayout.isRefreshing = false
            binding.scrollViewScoredFood.scrollTo(0,40)
            onTasksNotCompleted()


        }
        viewModel.mainFood.observe(viewLifecycleOwner){
            if(it){
                binding.card1ScoredFood.visibility = View.VISIBLE

            }else{
                binding.card1ScoredFood.visibility = View.GONE
            }
            controlMain = true
            viewModel.isLoading.value = true

        }


        viewModel.drinksFood.observe(viewLifecycleOwner){
            if(it){
                binding.card4ScoredFood.visibility = View.VISIBLE

            }else{
                binding.card4ScoredFood.visibility = View.GONE
            }
            controlDrinks = true
            viewModel.isLoading.value = true
        }

        viewModel.soapFood.observe(viewLifecycleOwner){
            if(it){
                binding.card2ScoredFood.visibility = View.VISIBLE

            }else{
                binding.card2ScoredFood.visibility = View.GONE
            }
            controlSoap = true
            viewModel.isLoading.value = true
        }
        viewModel.desertsFood.observe(viewLifecycleOwner){
            if(it){
                binding.card3ScoredFood.visibility = View.VISIBLE

            }else{
                binding.card3ScoredFood.visibility = View.GONE
            }
            controlDeserts = true
            viewModel.isLoading.value = true
        }
        viewModel.isLoading.observe(viewLifecycleOwner){
            if(controlMain&&controlDeserts&&controlDrinks&&controlSoap){
                onTasksCompleted()
            }
        }

        binding.scrollViewScoredFood.viewTreeObserver.addOnScrollChangedListener {
            if (binding.scrollViewScoredFood.scrollY <100) {
                // ScrollView en üstte, SwipeRefreshLayout'u etkinleştir.
                binding.swipeRefreshLayout.isEnabled = true
            } else {
                // ScrollView en üstte değil, SwipeRefreshLayout'u devre dışı bırak.
                binding.swipeRefreshLayout.isEnabled = false
            }
        }
        binding.scrollViewScoredFood.scrollTo(0,2)

        binding.mainFoodsMoreImageScoreFoods.setOnClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToMoreFoodsFragment(2 ,"Ana Yemek")
            findNavController().navigate(direction)
        }
        binding.soapFoodsMoreScoreFoods.setOnClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToMoreFoodsFragment(2 ,"Çorba")
            findNavController().navigate(direction)
        }
        binding.DesertsFoodsMoreScoreFoods.setOnClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToMoreFoodsFragment(2 ,"Tatlı")
            findNavController().navigate(direction)
        }
        binding.DrinksFoodsMoreScoreFoods.setOnClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToMoreFoodsFragment(2 ,"İçicek")
            findNavController().navigate(direction)
        }


        return binding.root
    }
    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {

        super.onViewCreated(view, savedInstanceState)
        onTasksNotCompleted()
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val tempViewModel: ScoredFoodViewModel by viewModels()

        viewModel = tempViewModel

    }

    fun createAdapter (){
        val mainFoodsAdapter = MainFoodsAdapter()
        val soapFoodsAdapter = MainFoodsAdapter()
        val drinksFoodsAdapter = MainFoodsAdapter()
        val desertsFoodsAdapter = MainFoodsAdapter()
        viewModel.changeLists()
        binding.apply {
            lifecycleScope.launch {
                viewModel.mainFoodsList.collect {
                    mainFoodsAdapter.submitData(it)
                }
            }
            lifecycleScope.launch {
                viewModel.soapFoodsList.collect {
                    soapFoodsAdapter.submitData(it)
                }
            }
            lifecycleScope.launch {
                viewModel.desertFoodList.collect {
                    desertsFoodsAdapter.submitData(it)
                }
            }
            lifecycleScope.launch {
                viewModel.drinksFoodList.collect {
                    drinksFoodsAdapter.submitData(it)
                }
            }
            lifecycleScope.launch {
                viewModel.drinksFoodList
            }

        }

        mainFoodsAdapter.setOnItemClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToFoodsDetailFragment(it)
            findNavController().navigate(direction)
        }
        soapFoodsAdapter.setOnItemClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToFoodsDetailFragment(it)
            findNavController().navigate(direction)
        }
        drinksFoodsAdapter.setOnItemClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToFoodsDetailFragment(it)
            findNavController().navigate(direction)
        }
        desertsFoodsAdapter.setOnItemClickListener {
            val direction = ScoredFoodFragmentDirections.actionScoredFoodFragmentToFoodsDetailFragment(it)
            findNavController().navigate(direction)
        }

        lifecycleScope.launch {
            mainFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMoviesScoredFood.isVisible = state is LoadState.Loading
            }
            soapFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMoviesScoredFood.isVisible = state is LoadState.Loading
            }
            drinksFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMoviesScoredFood.isVisible = state is LoadState.Loading
            }
            desertsFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMoviesScoredFood.isVisible = state is LoadState.Loading
            }
        }
        binding.mainFoods.apply {
            layoutManager = LinearLayoutManager(requireContext())
            layoutManager = StaggeredGridLayoutManager(1, StaggeredGridLayoutManager.HORIZONTAL)
            adapter = mainFoodsAdapter
        }

        binding.mainFoods.adapter=mainFoodsAdapter.withLoadStateFooter(
            LoadMoreAdapter{
                mainFoodsAdapter.retry()
            }
        )

        binding.mainFoods1.apply {
            layoutManager = LinearLayoutManager(requireContext())
            layoutManager = StaggeredGridLayoutManager(1, StaggeredGridLayoutManager.HORIZONTAL)
            adapter = soapFoodsAdapter
        }

        binding.mainFoods1.adapter=soapFoodsAdapter.withLoadStateFooter(
            LoadMoreAdapter{
                soapFoodsAdapter.retry()
            }
        )




        binding.mainFoods2.apply {
            layoutManager = LinearLayoutManager(requireContext())
            layoutManager = StaggeredGridLayoutManager(1, StaggeredGridLayoutManager.HORIZONTAL)
            adapter = desertsFoodsAdapter
        }

        binding.mainFoods2.adapter=desertsFoodsAdapter.withLoadStateFooter(
            LoadMoreAdapter{
                desertsFoodsAdapter.retry()
            }
        )
        binding.mainFoods3.apply {
            layoutManager = LinearLayoutManager(requireContext())
            layoutManager = StaggeredGridLayoutManager(1, StaggeredGridLayoutManager.HORIZONTAL)
            adapter = drinksFoodsAdapter
        }

        binding.mainFoods3.adapter=drinksFoodsAdapter.withLoadStateFooter(
            LoadMoreAdapter{
                drinksFoodsAdapter.retry()
            }
        )

    }


    override fun onResume() {
        super.onResume()
        onTasksNotCompleted()
    }
    private fun onTasksCompleted() {
        binding.progressBarScoredFood.visibility = View.INVISIBLE
        binding.progressCardScoredFood.visibility = View.INVISIBLE
    }
    private fun onTasksNotCompleted(){
        binding.progressBarScoredFood.visibility = View.VISIBLE
        binding.progressCardScoredFood.visibility = View.VISIBLE
        viewModel.checkUserIdExistsInScoredFoods()

        controlDeserts = false
        controlMain = false
        controlDrinks = false
        controlSoap = false
        createAdapter()
        viewModel.controlMainFoodsLists()
        viewModel.controlDrinksFoodsLists()
        viewModel.controlDesertsFoodsLists()
        viewModel.controlSoapFoodsLists()
    }


}