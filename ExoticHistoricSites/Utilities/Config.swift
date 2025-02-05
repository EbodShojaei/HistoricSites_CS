//
//  Config.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import Foundation

struct Config {
    static let shared = Config()
    
    private let configDict: [String: Any]
    
    private init() {
        guard let path = Bundle.main.path(forResource: "Config", ofType: "plist"),
              let dict = NSDictionary(contentsOfFile: path) as? [String: Any] else {
            fatalError("Config.plist not found")
        }
        self.configDict = dict
    }
    
    var apiBaseURL: String {
        guard let url = configDict["API_BASE_URL"] as? String else {
            fatalError("API_BASE_URL not found in Config.plist")
        }
        return url
    }
}
