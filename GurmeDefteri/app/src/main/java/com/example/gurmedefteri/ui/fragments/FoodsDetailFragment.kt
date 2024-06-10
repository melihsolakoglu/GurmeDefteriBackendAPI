package com.example.gurmedefteri.ui.fragments

import android.graphics.Bitmap
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.ViewTreeObserver
import android.view.animation.AnimationUtils
import android.widget.Toolbar
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.viewModels
import androidx.navigation.fragment.navArgs
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentFoodsDetailBinding
import com.example.gurmedefteri.ui.viewmodels.FoodsDetailViewModel
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class FoodsDetailFragment : Fragment() {
    private lateinit var binding:  FragmentFoodsDetailBinding
    private lateinit var viewModel: FoodsDetailViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentFoodsDetailBinding.inflate(inflater, container, false)

        var checkScoredFoodOk = false
        var getAverageScoreOk = false
        val bundle:FoodsDetailFragmentArgs by navArgs()
        val food = bundle.Food

        viewModel.checkScoredFood(viewModel.userId.value.toString(),food.id)
        viewModel.getAverageScoredFood(viewModel.userId.value.toString(),food.id)

        viewModel.getAverageScoreOk.observe(viewLifecycleOwner){
            getAverageScoreOk = it
        }
        viewModel.checkScoredFoodOk.observe(viewLifecycleOwner){
            checkScoredFoodOk = it
        }
        viewModel.scored.observe(viewLifecycleOwner){
            if(it){
                binding.textviewSuggestionForYou.visibility = View.GONE
                binding.cardViewSuggestionScore.visibility = View.GONE
            }else{
            }
        }
        viewModel.isLoading.observe(viewLifecycleOwner){
            if(checkScoredFoodOk && getAverageScoreOk){
                onTasksCompleted()
            }
        }
        viewModel.AverageScoreFood.observe(viewLifecycleOwner){
            if(it == 1){
                setStarsSuggestionScore(1)
            }else if(it == 2){
                setStarsSuggestionScore(2)
            }else if(it == 3){
                setStarsSuggestionScore(3)
            }else if(it == 4){
                setStarsSuggestionScore(4)
            }else if(it == 5){
                setStarsSuggestionScore(5)
            }else if(it == 6){
                setStarsSuggestionScore(6)
            }else if(it == 7){
                setStarsSuggestionScore(7)
            }else if(it == 8){
                setStarsSuggestionScore(8)
            }else if(it == 9){
                setStarsSuggestionScore(9)
            }else if(it == 10){
                setStarsSuggestionScore(10)
            }
        }



        binding.constraintFoodsDetail.viewTreeObserver.addOnGlobalLayoutListener(object : ViewTreeObserver.OnGlobalLayoutListener {
            override fun onGlobalLayout() {
                val width = binding.foodimageFoodDetail.width
                val height = binding.foodimageFoodDetail.height

                val bitmap = viewModel.base64ToBitmap(food.imageBytes)
                val resizedBitmap = Bitmap.createScaledBitmap(bitmap, width, height, true)
                binding.foodimageFoodDetail.setImageBitmap(resizedBitmap)
                binding.constraintFoodsDetail.viewTreeObserver.removeOnGlobalLayoutListener(this)
            }
        })


        val parentActivity = activity as? AppCompatActivity

        parentActivity?.supportActionBar?.apply {
            title = food.name
        }




        binding.foodCategory.setText(food.category)
        binding.foodCountry.setText(food.country)
        binding.foodDescription.setText(food.description)

        viewModel.scoreViewModel.observe(viewLifecycleOwner){
            if(it == 1){
                setStars(1)
            }else if(it ==2){
                setStars(2)
            }else if(it ==3){
                setStars(3)
            }else if(it ==4){
                setStars(4)
            }else if(it ==5){
                setStars(5)
            }else if(it ==6){
                setStars(6)
            }else if(it ==7){
                setStars(7)
            }else if(it ==8){
                setStars(8)
            }else if(it ==9){
                setStars(9)
            }else if(it ==10){
                setStars(10)
            }
        }

        binding.apply {
            yildiz1.setOnClickListener {
                if(viewModel.scoreViewModel.value==0){
                    viewModel.addScoredFoods(food.id,2)
                    viewModel.scoreViewModel.value =2
                }else{
                    if(viewModel.scoreViewModel.value == 2){
                        viewModel.updateScoredFoods(food.id,1)
                        viewModel.scoreViewModel.value = 1
                    }else{
                        viewModel.updateScoredFoods(food.id,2)
                        viewModel.scoreViewModel.value =2
                    }
                }

                viewModel.scored.value =true

            }
            yildiz2.setOnClickListener {
                if(viewModel.scoreViewModel.value==0){
                    viewModel.addScoredFoods(food.id,4)
                    viewModel.scoreViewModel.value =4
                }else{
                    if(viewModel.scoreViewModel.value == 4){
                        viewModel.updateScoredFoods(food.id,3)
                        viewModel.scoreViewModel.value =3
                    }else{
                        viewModel.updateScoredFoods(food.id,4)
                        viewModel.scoreViewModel.value =4
                    }
                }

                viewModel.scored.value =true
            }
            yildiz3.setOnClickListener {
                if(viewModel.scoreViewModel.value==0){
                    viewModel.addScoredFoods(food.id,6)
                    viewModel.scoreViewModel.value =6
                }else{
                    if(viewModel.scoreViewModel.value == 6){
                        viewModel.updateScoredFoods(food.id,5)
                        viewModel.scoreViewModel.value =5
                    }else{
                        viewModel.updateScoredFoods(food.id,6)
                        viewModel.scoreViewModel.value =6
                    }
                }

                viewModel.scored.value =true
            }

            yildiz4.setOnClickListener {
                if(viewModel.scoreViewModel.value==0){
                    viewModel.addScoredFoods(food.id,8)
                }else{
                    if(viewModel.scoreViewModel.value == 8){
                        viewModel.updateScoredFoods(food.id,7)
                        viewModel.scoreViewModel.value =7
                    }else{
                        viewModel.updateScoredFoods(food.id,8)
                        viewModel.scoreViewModel.value =8
                    }
                }

                viewModel.scored.value =true
            }
            yildiz5.setOnClickListener {
                if(viewModel.scoreViewModel.value==0){
                    viewModel.addScoredFoods(food.id,10)
                }else{
                    if(viewModel.scoreViewModel.value == 10){
                        viewModel.updateScoredFoods(food.id,9)
                        viewModel.scoreViewModel.value =9
                    }else{
                        viewModel.updateScoredFoods(food.id,10)
                        viewModel.scoreViewModel.value =10
                    }
                }
                viewModel.scored.value =true
            }
        }



        return binding.root
    }
    fun setStars(score:Int){
        val star1 = binding.yildiz1
        val star2 = binding.yildiz2
        val star3 = binding.yildiz3
        val star4 = binding.yildiz4
        val star5 = binding.yildiz5

        val emptyStar = R.drawable.drawing
        val halfStar = R.drawable.half_star
        val fullStar = R.drawable.full_star

        if(score ==1){
            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(emptyStar)
            star1.setImageResource(halfStar)
        }
        if(score ==2){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(emptyStar)
            star1.setImageResource(fullStar)
        }
        if(score ==3){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(halfStar)
            star1.setImageResource(fullStar)
        }
        if(score ==4){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==5){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(halfStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==6){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==7){

            star5.setImageResource(emptyStar)
            star4.setImageResource(halfStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==8){

            star5.setImageResource(emptyStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==9){

            star5.setImageResource(halfStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==10){

            star5.setImageResource(fullStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
    }

    fun setStarsSuggestionScore(score:Int){
        val star1 = binding.star1SuggestionScore
        val star2 = binding.star2SuggestionScore
        val star3 = binding.star3SuggestionScore
        val star4 = binding.star4SuggestionScore
        val star5 = binding.star5SuggestionScore

        val emptyStar = R.drawable.small_star_empty
        val halfStar = R.drawable.small_star_half
        val fullStar = R.drawable.smal_star_full

        if(score ==1){
            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(emptyStar)
            star1.setImageResource(halfStar)
        }
        if(score ==2){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(emptyStar)
            star1.setImageResource(fullStar)
        }
        if(score ==3){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(halfStar)
            star1.setImageResource(fullStar)
        }
        if(score ==4){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(emptyStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==5){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(halfStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==6){

            star5.setImageResource(emptyStar)
            star4.setImageResource(emptyStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==7){

            star5.setImageResource(emptyStar)
            star4.setImageResource(halfStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==8){

            star5.setImageResource(emptyStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==9){

            star5.setImageResource(halfStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
        if(score ==10){

            star5.setImageResource(fullStar)
            star4.setImageResource(fullStar)
            star3.setImageResource(fullStar)
            star2.setImageResource(fullStar)
            star1.setImageResource(fullStar)
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: FoodsDetailViewModel by viewModels()
        viewModel = tempViewModel
    }
    private fun onTasksCompleted() {
        binding.progressBarFoodDetail.visibility = View.GONE
        binding.progressCardFoodsDetail.visibility = View.GONE

        val cardSwipeAnimation = AnimationUtils.loadAnimation(context,R.anim.card_swipe_animation)
        val foodDetailImageAnimation = AnimationUtils.loadAnimation(context,R.anim.food_detail_image_animation)
        binding.foodimageFoodDetail.startAnimation(foodDetailImageAnimation)
        binding.cardSwiperFoodDetail.startAnimation(cardSwipeAnimation)

    }
}