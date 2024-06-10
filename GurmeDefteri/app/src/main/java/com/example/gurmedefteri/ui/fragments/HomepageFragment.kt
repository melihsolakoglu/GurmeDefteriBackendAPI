package com.example.gurmedefteri.ui.fragments

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.GestureDetector
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.MotionEvent
import android.view.View
import android.view.ViewGroup
import androidx.core.view.isVisible
import androidx.fragment.app.viewModels
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.NavHostFragment
import androidx.paging.LoadState
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.navigation.fragment.findNavController
import androidx.paging.PagingData
import androidx.recyclerview.widget.StaggeredGridLayoutManager
import com.example.gurmedefteri.MainActivity
import com.example.gurmedefteri.R
import com.example.gurmedefteri.SplashScreen
import com.example.gurmedefteri.SuggestionFood
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.databinding.FragmentHomepageBinding
import com.example.gurmedefteri.ui.adapters.LoadMoreAdapter
import com.example.gurmedefteri.ui.adapters.MainFoodsAdapter
import com.example.gurmedefteri.ui.adapters.SoapFoodsAdapter
import com.example.gurmedefteri.ui.viewmodels.HomepageViewModel
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.collectIndexed
import kotlinx.coroutines.flow.collectLatest
import kotlinx.coroutines.flow.firstOrNull
import kotlinx.coroutines.launch
import javax.inject.Inject

@AndroidEntryPoint
class HomepageFragment : Fragment() {
    private lateinit var binding: FragmentHomepageBinding
    private lateinit var viewModel: HomepageViewModel

    private var controlMain = false
    private var controlSoap = false
    private var controlDeserts = false
    private var controlDrinks = false

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        binding = FragmentHomepageBinding.inflate(inflater,container,false)

        viewModel.checkUserIdExistsInScoredFoods()
        viewModel.isScoredAnyFood.observe(viewLifecycleOwner){
            if(it){
                binding.SuggestionFoodImage.visibility =View.VISIBLE
            }else{

                binding.SuggestionFoodImage.visibility =View.INVISIBLE

            }
        }

        val swipeRefreslLayout = binding.swipeRefreshLayout
        swipeRefreslLayout.setOnRefreshListener {
            swipeRefreslLayout.isRefreshing = false
            binding.scrollView.scrollTo(0,40)
            onTasksNotCompleted()


        }
        viewModel.mainFood.observe(viewLifecycleOwner){
            if(it){
                binding.card1.visibility = View.VISIBLE

            }else{
                binding.card1.visibility = View.GONE
            }
            controlMain = true
            viewModel.isLoading.value = true

        }


        viewModel.drinksFood.observe(viewLifecycleOwner){
            if(it){
                binding.card4.visibility = View.VISIBLE

            }else{
                binding.card4.visibility = View.GONE
            }
            controlDrinks = true
            viewModel.isLoading.value = true
        }

        viewModel.soapFood.observe(viewLifecycleOwner){
            if(it){
                binding.card2.visibility = View.VISIBLE

            }else{
                binding.card2.visibility = View.GONE
            }
            controlSoap = true
            viewModel.isLoading.value = true
        }
        viewModel.desertsFood.observe(viewLifecycleOwner){
            if(it){
                binding.card3.visibility = View.VISIBLE

            }else{
                binding.card3.visibility = View.GONE
            }
            controlDeserts = true
            viewModel.isLoading.value = true
        }
        viewModel.isLoading.observe(viewLifecycleOwner){
            if(controlMain&&controlDeserts&&controlDrinks&&controlSoap){
                onTasksCompleted()
            }
        }

        binding.scrollView.viewTreeObserver.addOnScrollChangedListener {
            if (binding.scrollView.scrollY <100) {
                binding.swipeRefreshLayout.isEnabled = true
            } else {
                binding.swipeRefreshLayout.isEnabled = false
            }
        }
        binding.SuggestionFoodImage.setOnClickListener{
            //val direction = HomepageFragmentDirections.actionHomepageFragmentToSuggestionFoodFragment()
            //findNavController().navigate(direction)
            val homeIntent = Intent(context, SuggestionFood::class.java)
            startActivity(homeIntent)
        }
        binding.scrollView.scrollTo(0,2)

        binding.mainFoodsMoreImage.setOnClickListener {
            val direction = HomepageFragmentDirections.actionHomepageFragmentToMoreFoodsFragment(1 ,"Ana Yemek")
            findNavController().navigate(direction)
        }
        binding.soapFoodsMore.setOnClickListener {
            val direction = HomepageFragmentDirections.actionHomepageFragmentToMoreFoodsFragment(1 ,"Çorba")
            findNavController().navigate(direction)
        }
        binding.DesertsFoodsMore.setOnClickListener {
            val direction = HomepageFragmentDirections.actionHomepageFragmentToMoreFoodsFragment(1 ,"Tatlı")
            findNavController().navigate(direction)
        }
        binding.DrinksFoodsMore.setOnClickListener {
            val direction = HomepageFragmentDirections.actionHomepageFragmentToMoreFoodsFragment(1 ,"İçicek")
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
        val tempViewModel: HomepageViewModel by viewModels()
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
            val direction = HomepageFragmentDirections.actionHomepageToFoodsDetail(it)
            findNavController().navigate(direction)
        }
        soapFoodsAdapter.setOnItemClickListener {
            val direction = HomepageFragmentDirections.actionHomepageToFoodsDetail(it)
            findNavController().navigate(direction)
        }
        drinksFoodsAdapter.setOnItemClickListener {
            val direction = HomepageFragmentDirections.actionHomepageToFoodsDetail(it)
            findNavController().navigate(direction)
        }
        desertsFoodsAdapter.setOnItemClickListener {
            val direction = HomepageFragmentDirections.actionHomepageToFoodsDetail(it)
            findNavController().navigate(direction)
        }

        lifecycleScope.launch {
            mainFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMovies.isVisible = state is LoadState.Loading
            }
            soapFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMovies.isVisible = state is LoadState.Loading
            }
            drinksFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMovies.isVisible = state is LoadState.Loading
            }
            desertsFoodsAdapter.loadStateFlow.collect{
                val state = it.refresh
                binding.prgBarMovies.isVisible = state is LoadState.Loading
            }
        }
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
        binding.progressBarHomepage.visibility = View.INVISIBLE
        binding.progressCardHomepage.visibility = View.INVISIBLE
    }
    private fun onTasksNotCompleted(){
        binding.progressBarHomepage.visibility = View.VISIBLE
        binding.progressCardHomepage.visibility = View.VISIBLE

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