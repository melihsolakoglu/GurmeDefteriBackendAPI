package com.example.gurmedefteri.ui.fragments

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.DialogFragment
import androidx.recyclerview.widget.RecyclerView
import com.example.gurmedefteri.R
import dagger.hilt.android.AndroidEntryPoint

@AndroidEntryPoint
class SearchDialogFragment : DialogFragment() {

    private lateinit var query: String
    private lateinit var recyclerView: RecyclerView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        query = arguments?.getString(ARG_QUERY) ?: ""
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_search_dialog, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)


        // Arama sonuçlarını yükle ve adaptere bağla
        val results = searchItems(query)
    }

    private fun searchItems(query: String): List<String> {
        // Örnek olarak arama sonuçları
        return listOf("Item 1", "Item 2", "Item 3").filter { it.contains(query, ignoreCase = true) }
    }

    companion object {
        private const val ARG_QUERY = "query"

        fun newInstance(query: String): SearchDialogFragment {
            val fragment = SearchDialogFragment()
            val args = Bundle()
            args.putString(ARG_QUERY, query)
            fragment.arguments = args
            return fragment
        }
    }
}