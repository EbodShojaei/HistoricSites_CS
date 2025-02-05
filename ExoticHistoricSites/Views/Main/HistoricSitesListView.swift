//
//  HistoricSitesListView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct HistoricSitesListView: View {
    @EnvironmentObject private var viewModel: HistoricSitesViewModel
    @State private var showingAddSite = false
    
    var body: some View {
        NavigationView {
            Group {
                if viewModel.isLoading {
                    LoadingView()
                } else if let error = viewModel.error {
                    ErrorView(message: error) {
                        Task {
                            await viewModel.loadHistoricSites()
                        }
                    }
                } else {
                    List(viewModel.historicSites) { site in
                        NavigationLink(destination: HistoricSiteDetailView(site: site)) {
                            HistoricSiteRowView(site: site)
                        }
                        .swipeActions(edge: .trailing, allowsFullSwipe: false) {
                            Button(role: .destructive) {
                                Task {
                                    await viewModel.deleteHistoricSite(id: site.id)
                                }
                            } label: {
                                Label("Delete", systemImage: "trash")
                            }
                        }
                    }
                    .refreshable {
                        await viewModel.loadHistoricSites()
                    }
                }
            }
            .navigationTitle("Historic Sites")
            .toolbar {
                ToolbarItem(placement: .navigationBarTrailing) {
                    Button {
                        showingAddSite = true
                    } label: {
                        Image(systemName: "plus")
                    }
                }
            }
            .sheet(isPresented: $showingAddSite) {
                EditHistoricSiteView()
            }
        }
        .task {
            await viewModel.loadHistoricSites()
        }
    }
}
