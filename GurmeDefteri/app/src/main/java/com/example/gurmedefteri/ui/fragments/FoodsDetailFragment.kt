package com.example.gurmedefteri.ui.fragments

import android.graphics.Bitmap
import android.graphics.Color
import android.os.Bundle
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.ViewTreeObserver
import android.widget.Toolbar
import androidx.constraintlayout.widget.ConstraintLayout
import androidx.fragment.app.viewModels
import androidx.navigation.fragment.navArgs
import com.example.gurmedefteri.R
import com.example.gurmedefteri.databinding.FragmentFoodsDetailBinding
import com.example.gurmedefteri.ui.viewmodels.FoodsDetailViewModel
import com.google.android.material.snackbar.Snackbar
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
        val bundle:FoodsDetailFragmentArgs by navArgs()
        val food = bundle.Food
        viewModel.checkScoredFood(viewModel.userId.value.toString(),food.id)
        binding.constraintFoodsDetail.viewTreeObserver.addOnGlobalLayoutListener(object : ViewTreeObserver.OnGlobalLayoutListener {
            override fun onGlobalLayout() {
                // ConstraintLayout ve imageView ölçülerini alın
                val width = binding.imageView.width
                val height = binding.imageView.height
                Log.d("said", width.toString())
                Log.d("said", height.toString())

                val bitmap = viewModel.base64ToBitmap(food.imageBytes)
                val resizedBitmap = Bitmap.createScaledBitmap(bitmap, width, height, true)
                binding.imageView.setImageBitmap(resizedBitmap)
                binding.constraintFoodsDetail.viewTreeObserver.removeOnGlobalLayoutListener(this)
            }
        })







        binding.foodCategory.setText(food.category)
        binding.foodCountry.setText(food.country)
        binding.foodName.setText(food.name)

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

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val tempViewModel: FoodsDetailViewModel by viewModels()
        viewModel = tempViewModel
    }


}