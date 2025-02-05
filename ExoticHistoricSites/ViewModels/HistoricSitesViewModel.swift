//
//  HistoricSitesViewModel.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import Foundation
import UIKit
import SwiftUI

@MainActor
class HistoricSitesViewModel: ObservableObject {
    @Published var historicSites: [HistoricSite] = []
    @Published var selectedSite: HistoricSite?
    @Published var isLoading = false
    @Published var error: String?
    @Published var searchText = ""
    @Published var selectedCountry: String?
    
    private let apiService = APIService.shared
    
    // MARK: - Read Operations
    func loadHistoricSites() async {
        isLoading = true
        do {
            historicSites = try await apiService.fetchHistoricSites()
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    func loadHistoricSite(id: Int) async {
        isLoading = true
        do {
            selectedSite = try await apiService.fetchHistoricSite(id: id)
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    func searchSites() async {
        isLoading = true
        do {
            historicSites = try await apiService.searchHistoricSites(
                country: selectedCountry,
                name: searchText.isEmpty ? nil : searchText
            )
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    // MARK: - Create Operations
    func createHistoricSite(_ site: HistoricSite) async {
        isLoading = true
        do {
            let createdSite = try await apiService.createHistoricSite(site)
            historicSites.append(createdSite)
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    // MARK: - Update Operations
    func updateHistoricSite(_ site: HistoricSite) async {
        isLoading = true
        do {
            let updatedSite = try await apiService.updateHistoricSite(site)
            if let index = historicSites.firstIndex(where: { $0.id == updatedSite.id }) {
                historicSites[index] = updatedSite
            }
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    // MARK: - Delete Operations
    func deleteHistoricSite(id: Int) async {
        isLoading = true
        do {
            try await apiService.deleteHistoricSite(id: id)
            // Remove the site from the local array
            historicSites.removeAll { $0.id == id }
            error = nil
        } catch {
            self.error = error.localizedDescription
            // Refresh the list to ensure in sync with the server
            await loadHistoricSites()
        }
        isLoading = false
    }
    
    // MARK: - Image Operations
    func uploadImage(for siteId: Int, uiImage: UIImage) async {
        isLoading = true
        do {
            guard let imageData = uiImage.jpegData(compressionQuality: 0.8) else {
                throw NSError(domain: "", code: -1, userInfo: [NSLocalizedDescriptionKey: "Failed to convert image to data"])
            }
            
            let base64String = try await apiService.uploadImage(siteId: siteId, imageData: imageData)
            
            if let index = historicSites.firstIndex(where: { $0.id == siteId }) {
                var updatedSite = historicSites[index]
                updatedSite = updatedSite.updated(imageBase64: base64String)
                historicSites[index] = updatedSite
            }
            error = nil
        } catch {
            self.error = error.localizedDescription
        }
        isLoading = false
    }
    
    // MARK: - Helper Methods
    func clearError() {
        error = nil
    }
    
    func refreshData() async {
        await loadHistoricSites()
    }
}
