//
//  SearchBar.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import SwiftUI

struct SearchBar: View {
    @Binding var text: String
    @Binding var selectedCountry: String?
    let onSearch: () -> Void
    
    let countries = ["All", "China", "Jordan", "Vietnam", "Egypt", "Iran", "India", "Peru", "United Kingdom", "Ethiopia", "Mexico"]
    
    var body: some View {
        VStack(spacing: 12) {
            HStack {
                Image(systemName: "magnifyingglass")
                    .foregroundColor(.gray)
                
                TextField("Search sites...", text: $text)
                    .textFieldStyle(RoundedBorderTextFieldStyle())
                    .autocapitalization(.none)
                    .onChange(of: text) { oldValue, newValue in
                        onSearch()
                    }
                
                if !text.isEmpty {
                    Button(action: {
                        text = ""
                        onSearch()
                    }) {
                        Image(systemName: "xmark.circle.fill")
                            .foregroundColor(.gray)
                    }
                }
            }
            
            ScrollView(.horizontal, showsIndicators: false) {
                HStack {
                    ForEach(countries, id: \.self) { country in
                        CountryFilterButton(
                            country: country,
                            isSelected: country == selectedCountry,
                            action: {
                                selectedCountry = country == "All" ? nil : country
                                onSearch()
                            }
                        )
                    }
                }
            }
        }
    }
}

struct CountryFilterButton: View {
    let country: String
    let isSelected: Bool
    let action: () -> Void
    
    var body: some View {
        Button(action: action) {
            Text(country)
                .padding(.horizontal, 12)
                .padding(.vertical, 6)
                .background(isSelected ? Color.blue : Color.gray.opacity(0.2))
                .foregroundColor(isSelected ? .white : .primary)
                .cornerRadius(20)
        }
    }
}
