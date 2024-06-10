package com.example.gurmedefteri

import android.app.SearchManager
import android.content.Context
import android.content.Intent
import android.graphics.PorterDuff
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.view.View
import android.view.animation.Animation
import android.view.animation.AnimationUtils
import android.view.animation.RotateAnimation
import android.widget.EditText
import android.widget.PopupMenu
import android.widget.TextView
import androidx.activity.viewModels
import androidx.appcompat.app.ActionBarDrawerToggle
import androidx.appcompat.widget.SearchView
import androidx.appcompat.widget.Toolbar
import androidx.core.content.ContextCompat
import androidx.core.view.GravityCompat
import androidx.drawerlayout.widget.DrawerLayout
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.Observer
import androidx.navigation.NavController
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.NavigationUI
import com.example.gurmedefteri.databinding.ActivityMainBinding
import com.example.gurmedefteri.ui.viewmodels.MainViewModel
import com.example.gurmedefteri.ui.viewmodels.SearchFoodsViewModel
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class MainActivity : AppCompatActivity() /*, NavigationView.OnNavigationItemSelectedListener*/{
    private lateinit var binding: ActivityMainBinding
    private lateinit var navController: NavController
    private val viewModel: MainViewModel by viewModels()
    private val searchViewModel: SearchFoodsViewModel by viewModels()

    private lateinit var drawerLayout: DrawerLayout
    private lateinit var toggle: ActionBarDrawerToggle
    private lateinit var toolbar: Toolbar
    private lateinit var searchView: SearchView
    private var isDrawerOpened = false

    private var isSearchVisible = false
    private var isOnAFood = false
    private var anan = MutableLiveData<Int>()


    override fun onCreate(savedInstanceState: Bundle?) {
        var UserId = ""
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)



        val navHostFragment = supportFragmentManager.findFragmentById(R.id.nav_host_fragment_container) as NavHostFragment
        navController = navHostFragment.navController
        drawerLayout = binding.mainDrawerLayout
        toolbar = binding.toolbarMainActivity


        setSupportActionBar(toolbar)



        toggle = ActionBarDrawerToggle(this, drawerLayout,toolbar,R.string.open_nav,R.string.close_nav)
        drawerLayout.addDrawerListener(toggle)
        toggle.drawerArrowDrawable?.setColorFilter(ContextCompat.getColor(this, R.color.white), PorterDuff.Mode.SRC_ATOP)
        toggle.syncState()
        NavigationUI.setupWithNavController(binding.NavigationBarMain, navHostFragment.navController )
        viewModel.user.observe(this){
            val user = it
            try {

                val name =findViewById<TextView>(R.id.drawer_name)
                name.text = user?.name
                val email =findViewById<TextView>(R.id.drawer_email)
                email.text = user?.email
            }catch (e:Exception){

            }
        }

        viewModel.getUserId.observe(this, Observer { Id ->
            UserId = Id
            viewModel.getUserById(UserId)
        })

        drawerLayout.addDrawerListener(object : DrawerLayout.SimpleDrawerListener() {
            override fun onDrawerOpened(drawerView: View) {
                super.onDrawerOpened(drawerView)
                isDrawerOpened = true
            }

            override fun onDrawerClosed(drawerView: View) {
                super.onDrawerClosed(drawerView)

                isDrawerOpened = false
            }
        })

        searchViewModel.isOnFoodSearch.observe(this){
            isOnAFood =it
        }

        viewModel.loggedOut.observe(this){
            try {
                val loggedOut = it
                if(loggedOut){
                    val intent = Intent(this, SplashScreen::class.java)
                    intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP or Intent.FLAG_ACTIVITY_NEW_TASK)
                    startActivity(intent)
                    this?.finish()
                }
                else{

                }

            }catch (e:Exception){

            }
        }




        setupObservers()
        setContentView(binding.root)
    }
    private fun setupObservers() {
        searchViewModel.query.observe(this, Observer { query ->
        })
    }
    override fun onCreateOptionsMenu(menu: Menu): Boolean {

        menuInflater.inflate(R.menu.menu_toolbar, menu)
        val searchItem = menu.findItem(R.id.action_search)
        searchView = searchItem.actionView as SearchView
        val editText = searchView.findViewById<EditText>(androidx.appcompat.R.id.search_src_text)
        editText.setTextColor(resources.getColor(R.color.white))




        navController.addOnDestinationChangedListener { controller, destination, arguments ->
            when (destination.id) {
                R.id.homepageFragment -> {
                    searchItem.isVisible = true
                    toolbar.title = "GURME DEFTERİ"
                }

                R.id.profileFragment -> {
                    searchItem.isVisible = false
                    toolbar.title = getString(R.string.profile)
                }
                R.id.searchFoodsFragment -> {
                    searchItem.isVisible = true
                    toolbar.title = ""
                }

                R.id.scoredFoodFragment -> {
                    searchItem.isVisible = false
                    toolbar.title = getString(R.string.scoredFood)
                }
                R.id.moreFoodsFragment ->{
                    toggle.isDrawerIndicatorEnabled = false
                    searchItem.isVisible = false
                    isSearchVisible = true
                    supportActionBar?.setDisplayHomeAsUpEnabled(true)
                    toolbar.setNavigationIcon(R.drawable.baseline_arrow_back_24)
                    toolbar.setNavigationOnClickListener {
                        if(isSearchVisible ){
                            supportActionBar?.setDisplayHomeAsUpEnabled(false)

                            closeSearchView()
                            anan.value = 1
                        }else{
                            if (isDrawerOpened) {
                                // Drawer açıksa, kapat
                                drawerLayout.closeDrawer(GravityCompat.START)
                                isDrawerOpened = false
                                anan.value = 1
                            } else {
                                drawerLayout.openDrawer(GravityCompat.START)
                                isDrawerOpened = true
                                anan.value = 1
                            }

                        }




                    }
                    toggle.syncState()
                }
                R.id.foodsDetailFragment ->{
                    isOnAFood = true
                    supportActionBar?.setDisplayHomeAsUpEnabled(true)
                    searchItem.isVisible = false
                    toolbar.setNavigationIcon(R.drawable.baseline_arrow_back_24)
                    toolbar.setNavigationOnClickListener {



                        if(isSearchVisible){
                            if(isOnAFood){
                                navController.popBackStack()
                                isOnAFood =false
                                anan.value = 1
                            }else{
                                supportActionBar?.setDisplayHomeAsUpEnabled(false)
                                closeSearchView()
                                toggle.syncState()
                                anan.value = 1
                            }
                        }else{
                            if (isOnAFood){
                                supportActionBar?.setDisplayHomeAsUpEnabled(false)
                                navController.popBackStack()
                                isOnAFood =false

                                toggle.syncState()
                                anan.value = 1
                            }else{
                                if (isDrawerOpened) {
                                    // Drawer açıksa, kapat
                                    drawerLayout.closeDrawer(GravityCompat.START)
                                    isDrawerOpened = false
                                    anan.value = 1
                                } else {
                                    drawerLayout.openDrawer(GravityCompat.START)
                                    isDrawerOpened = true
                                    anan.value = 1
                                }
                            }
                        }

                    }
                }
                // Diğer fragment'lar için ekleme yapabilirsiniz
                else -> {
                    Log.d("NavController", "Other Fragment")
                }
            }
        }

        searchView.setOnSearchClickListener {
            toggle.isDrawerIndicatorEnabled = false
            isSearchVisible = true
            supportActionBar?.setDisplayHomeAsUpEnabled(true)
            toolbar.setNavigationIcon(R.drawable.baseline_arrow_back_24)
            navController.navigate(R.id.searchFoodsFragment)
            toolbar.setNavigationOnClickListener {
                if(isSearchVisible ){
                    supportActionBar?.setDisplayHomeAsUpEnabled(false)
                    closeSearchView()
                    anan.value = 1
                }else{
                    if (isDrawerOpened) {
                        drawerLayout.closeDrawer(GravityCompat.START)
                        isDrawerOpened = false
                        anan.value = 1
                    } else {
                        drawerLayout.openDrawer(GravityCompat.START)
                        isDrawerOpened = true
                        anan.value = 1
                    }

                }




            }
            toggle.syncState()

        }

        searchView.setOnCloseListener {
            toggle.isDrawerIndicatorEnabled = true
            supportActionBar?.setDisplayShowTitleEnabled(true)
            supportActionBar?.setDisplayHomeAsUpEnabled(false)
            isSearchVisible = false
            navController.popBackStack()
            toggle.syncState()
            false

        }

        searchView.setOnQueryTextListener(object : SearchView.OnQueryTextListener {
            override fun onQueryTextSubmit(query: String?): Boolean {
                query?.let {
                    searchViewModel.query.value = it
                }
                return true
            }

            override fun onQueryTextChange(newText: String?): Boolean {

                return false
            }
        })

        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        when (item.itemId) {
            android.R.id.home -> {
                onBackPressed()
                return true
            }
        }
        return super.onOptionsItemSelected(item)
    }

    override fun onBackPressed() {


        if(isSearchVisible){
            if(isOnAFood){
                navController.popBackStack()
                isOnAFood = false
            }else{
                closeSearchView()
                anan.value = 1
            }
        }else{
            if (isOnAFood){

                closeSearchView()
                isOnAFood =false
                anan.value = 1
            }else{
                super.onBackPressed()
            }
        }





    }
    private fun closeSearchView() {

        searchView?.setQuery("", false)
        searchView?.isIconified = true
    }

}