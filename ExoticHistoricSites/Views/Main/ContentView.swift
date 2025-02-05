//
//  ContentView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct ContentView: View {
    @StateObject private var viewModel = HistoricSitesViewModel()
    
    var body: some View {
        TabView {
            HistoricSitesListView()
                .tabItem {
                    Label("Sites", systemImage: "list.bullet")
                }
            
            SearchView()
                .tabItem {
                    Label("Search", systemImage: "magnifyingglass")
                }
            
            MapView()
                .tabItem {
                    Label("Map", systemImage: "map")
                }
        }
        .environmentObject(viewModel)
    }
}
