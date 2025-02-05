//
//  SearchView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct SearchView: View {
    @EnvironmentObject private var viewModel: HistoricSitesViewModel
    
    var body: some View {
        NavigationView {
            VStack(spacing: 0) {
                // Search Bar
                SearchBar(
                    text: $viewModel.searchText,
                    selectedCountry: $viewModel.selectedCountry
                ) {
                    Task {
                        await viewModel.searchSites()
                    }
                }
                .padding()
                
                // Results
                if viewModel.isLoading {
                    LoadingView()
                } else if let error = viewModel.error {
                    ErrorView(message: error) {
                        Task {
                            await viewModel.searchSites()
                        }
                    }
                } else {
                    List(viewModel.historicSites) { site in
                        NavigationLink(destination: HistoricSiteDetailView(site: site)) {
                            HistoricSiteRowView(site: site)
                        }
                    }
                    .listStyle(PlainListStyle())
                }
            }
            .navigationTitle("Search")
        }
    }
}
