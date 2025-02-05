//
//  EditHistoricSiteView.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-17.
//

import SwiftUI

struct EditHistoricSiteView: View {
    @Environment(\.dismiss) private var dismiss
    @EnvironmentObject private var viewModel: HistoricSitesViewModel
    
    @State private var name: String = ""
    @State private var description: String = ""
    @State private var countries: String = ""
    @State private var latitude: String = ""
    @State private var longitude: String = ""
    
    let isEditing: Bool
    let existingSite: HistoricSite?
    
    init(site: HistoricSite? = nil) {
        self.isEditing = site != nil
        self.existingSite = site
        
        if let site = site {
            _name = State(initialValue: site.name)
            _description = State(initialValue: site.description)
            _countries = State(initialValue: site.countries)
            _latitude = State(initialValue: String(site.latitude))
            _longitude = State(initialValue: String(site.longitude))
        }
    }
    
    var body: some View {
        NavigationView {
            Form {
                Section(header: Text("Basic Information")) {
                    TextField("Name", text: $name)
                    TextField("Description", text: $description, axis: .vertical)
                        .lineLimit(4...6)
                    TextField("Countries", text: $countries)
                }
                
                Section(header: Text("Location")) {
                    TextField("Latitude", text: $latitude)
                        .keyboardType(.decimalPad)
                    TextField("Longitude", text: $longitude)
                        .keyboardType(.decimalPad)
                }
            }
            .navigationTitle(isEditing ? "Edit Site" : "Add Site")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Cancel") {
                        dismiss()
                    }
                }
                ToolbarItem(placement: .confirmationAction) {
                    Button("Save") {
                        saveChanges()
                    }
                    .disabled(!isFormValid)
                }
            }
        }
    }
    
    private var isFormValid: Bool {
        !name.isEmpty &&
        !description.isEmpty &&
        !countries.isEmpty &&
        Double(latitude) != nil &&
        Double(longitude) != nil
    }
    
    private func saveChanges() {
        guard let lat = Double(latitude),
              let lon = Double(longitude) else { return }
        
        Task {
            if isEditing, let existingSite = existingSite {
                let updatedSite = existingSite.updated(
                    name: name,
                    description: description,
                    countries: countries,
                    latitude: lat,
                    longitude: lon
                )
                await viewModel.updateHistoricSite(updatedSite)
            } else {
                let newSite = HistoricSite(
                    id: 0,
                    name: name,
                    description: description,
                    countries: countries,
                    latitude: lat,
                    longitude: lon,
                    imageBase64: nil,
                    averageRating: 5.0,
                    totalReviews: 0
                )
                await viewModel.createHistoricSite(newSite)
            }
            dismiss()
        }
    }
}
