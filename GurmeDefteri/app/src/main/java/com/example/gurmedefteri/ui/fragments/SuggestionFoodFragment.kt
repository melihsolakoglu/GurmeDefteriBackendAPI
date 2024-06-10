package com.example.gurmedefteri.ui.fragments

import android.graphics.Bitmap
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.ViewTreeObserver
import android.view.animation.Animation
import android.view.animation.AnimationUtils
import android.widget.TextView
import androidx.fragment.app.viewModels
import com.example.gurmedefteri.R
import com.example.gurmedefteri.data.entity.Food
import com.example.gurmedefteri.data.entity.SuggestionFood
import com.example.gurmedefteri.databinding.FragmentSuggestionFoodBinding
import com.example.gurmedefteri.ui.viewmodels.SigninScreenViewModel
import com.example.gurmedefteri.ui.viewmodels.SuggestionFoodViewModel
import dagger.hilt.android.AndroidEntryPoint
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

@AndroidEntryPoint
class SuggestionFoodFragment : Fragment() {
    private lateinit var binding: FragmentSuggestionFoodBinding
    private lateinit var viewModel: SuggestionFoodViewModel
    private lateinit var food : SuggestionFood

    private val typingText = "DAMAK ZEVKİNİZİ HESAPLIYORUZ..."
    private lateinit var textviewForYou: TextView

    private val handler = Handler(Looper.getMainLooper())

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        binding = FragmentSuggestionFoodBinding.inflate(inflater, container, false)
        viewModel.getFoodScoreSuggestion(viewModel.userId.value.toString())
        val topAnimation = AnimationUtils.loadAnimation(context,R.anim.gone_animation)

        textviewForYou = binding.suggestionFoodSearching
        startTypeWriterEffect()

        topAnimation.setAnimationListener(object : Animation.AnimationListener {
            override fun onAnimationStart(animation: Animation?) {}

            override fun onAnimationEnd(animation: Animation?) {
                // Animasyon tamamlandığında cardSuggestion görünümünü gizleyin
                binding.cardSuggestion.visibility = View.GONE
            }

            override fun onAnimationRepeat(animation: Animation?) {}
        })

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
            yildiz1Suggestion.setOnClickListener {
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
                viewModel.destroySuggestionScore.value = true

            }
            yildiz2Suggestion.setOnClickListener {
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
                viewModel.destroySuggestionScore.value = true
            }
            yildiz3Suggestion.setOnClickListener {
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
                viewModel.destroySuggestionScore.value = true
            }

            yildiz4Suggestion.setOnClickListener {
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
                viewModel.destroySuggestionScore.value = true
            }
            yildiz5Suggestion.setOnClickListener {
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
                viewModel.destroySuggestionScore.value = true
            }
        }
        viewModel.destroySuggestionScore.observe(viewLifecycleOwner){
            if(it){
                binding.cardViewSuggestionScore.visibility = View.GONE
                binding.textviewSuggestionForYou.visibility = View.GONE
            }
        }

        viewModel.getSuggestionFood.observe(viewLifecycleOwner){
            food = it
            getFoodContents()
            binding.cardSuggestion.startAnimation(topAnimation)

        }


        return binding.root
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: SuggestionFoodViewModel by viewModels()
        viewModel = tempViewModel
    }


    fun getFoodContents(){
        binding.foodNameSuggestion.text = food.name
        binding.foodCategorySuggestion.text = food.category
        binding.foodCountrySuggestion.text = food.country
        binding.foodDescriptionSuggestion.text = food.description

        binding.constraintFoodSuggestion.viewTreeObserver.addOnGlobalLayoutListener(object : ViewTreeObserver.OnGlobalLayoutListener {
            override fun onGlobalLayout() {
                val width = binding.foodimageSuggestionFood.width
                val height = binding.foodimageSuggestionFood.height

                val bitmap = viewModel.base64ToBitmap(food.imageBytes)
                val resizedBitmap = Bitmap.createScaledBitmap(bitmap, width, height, true)
                binding.foodimageSuggestionFood.setImageBitmap(resizedBitmap)
                binding.constraintFoodSuggestion.viewTreeObserver.removeOnGlobalLayoutListener(this)
            }
        })
        val foodScore = food.score
        if(foodScore == 1){
            setStarsSuggestionScore(1)
        }else if(foodScore == 2){
            setStarsSuggestionScore(2)
        }else if(foodScore == 3){
            setStarsSuggestionScore(3)
        }else if(foodScore == 4){
            setStarsSuggestionScore(4)
        }else if(foodScore == 5){
            setStarsSuggestionScore(5)
        }else if(foodScore == 6){
            setStarsSuggestionScore(6)
        }else if(foodScore == 7){
            setStarsSuggestionScore(7)
        }else if(foodScore == 8){
            setStarsSuggestionScore(8)
        }else if(foodScore == 9){
            setStarsSuggestionScore(9)
        }else if(foodScore == 10){
            setStarsSuggestionScore(10)
        }


    }

    fun setStars(score:Int){
        val star1 = binding.yildiz1Suggestion
        val star2 = binding.yildiz2Suggestion
        val star3 = binding.yildiz3Suggestion
        val star4 = binding.yildiz4Suggestion
        val star5 = binding.yildiz5Suggestion

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

    private fun startTypeWriterEffect() {
        val delayMillis: Long = 150 // Her karakter için gecikme süresi (milisaniye)
        var index = 0

        val runnable = object : Runnable {
            override fun run() {
                if (index < typingText.length) {
                    textviewForYou.text = typingText.substring(0, index + 1)
                    index++
                    handler.postDelayed(this, delayMillis)
                } else {
                    handler.postDelayed({
                        textviewForYou.text = ""
                        index = 0
                        handler.post(this)
                    }, 2000) // Tamamlanan yazıdan sonra 2 saniye bekleme
                }
            }
        }
        handler.post(runnable)
    }


}