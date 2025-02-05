//
//  UIImage+Base64.swift
//  ExoticHistoricSites
//
//  Created by Ebod Shojaei on 2024-11-16.
//

import UIKit

extension UIImage {
    static func fromBase64(_ base64String: String?) -> UIImage? {
        guard let base64String = base64String else { return nil }
        
        // Remove data URI scheme if present
        let cleanedString = base64String.replacingOccurrences(of: "data:image/jpeg;base64,", with: "")
        
        guard let imageData = Data(base64Encoded: cleanedString) else { return nil }
        return UIImage(data: imageData)
    }
}
