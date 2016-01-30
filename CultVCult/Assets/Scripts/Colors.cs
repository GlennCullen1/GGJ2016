using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Colors
{
	public enum ColorNames
	{
		WASTE = -1,
		RED = 0,
		YELLOW,
		BLUE,
		PRIMARY = BLUE,
		GREEN,
		ORANGE,
		PURPLE,
		SECONDARY = PURPLE,
		TURQUOISE,
		CHARTREUSE,
		VERMILLION,
		AMBER,
		BYZANTIUM,
		PERIWINKLE,
		MAX_COLOR
	};

	private static ColorNames[,] mixes = 	// primary+1 x secondary+1
	{
		{ ColorNames.RED, ColorNames.ORANGE, ColorNames.PURPLE, ColorNames.WASTE, ColorNames.VERMILLION, ColorNames.BYZANTIUM },
		{ ColorNames.ORANGE, ColorNames.GREEN, ColorNames.YELLOW, ColorNames.CHARTREUSE, ColorNames.AMBER, ColorNames.WASTE },
		{ ColorNames.PURPLE, ColorNames.BLUE, ColorNames.GREEN, ColorNames.TURQUOISE, ColorNames.WASTE, ColorNames.PERIWINKLE }
	};

	private static uint[] colorRGBs = 
	{
		0xD63B2BFF,	0xFFEA3DFF,	0x3794EBFF,	0x09921CFF,	0xE9660FFF,	0x6F29EEFF,	
		0x0ECCD1FF,	0xC5F913FF,	0xA73306FF,	0xE88E0EFF,	0xCC2457FF,	0x7F7FF4FF
	};
	
	private static Dictionary<Color, ColorNames> floatToNames = new Dictionary<Color, ColorNames>
	{
		{ HexToColor(0xD63B2BFF), ColorNames.RED }, { HexToColor(0xFFEA3DFF), ColorNames.YELLOW}, { HexToColor(0x3794EBFF), ColorNames.BLUE },
		{ HexToColor(0x09921CFF), ColorNames.GREEN }, { HexToColor(0xE9660FFF), ColorNames.ORANGE}, { HexToColor(0x6F29EEFF), ColorNames.PURPLE },
		{ HexToColor(0x0ECCD1FF), ColorNames.TURQUOISE }, { HexToColor(0xC5F913FF), ColorNames.CHARTREUSE}, { HexToColor(0xA73306FF), ColorNames.VERMILLION },
		{ HexToColor(0xE88E0EFF), ColorNames.AMBER }, { HexToColor(0xCC2457FF), ColorNames.BYZANTIUM}, { HexToColor(0x7F7FF4FF), ColorNames.PERIWINKLE },
		
	};
	
	/// <summary>
	/// Mixs the colors. 
	/// </summary>
	/// <returns>The colors. If an unacceptable colour is sent in, </returns>
	/// <param name="primary">Primary.</param>
	/// <param name="other">Other.</param>
	public static Color MixColors(Color primary, Color other)
	{
		// Color notThere = new Color(1, 1, 1, 0);

//		if (!floatToNames.ContainsKey(primary)  
//		    || floatToNames[primary] > ColorNames.PRIMARY 
//		    || floatToNames[other] > ColorNames.SECONDARY)
//		{
//			return notThere;
//		}
		
		uint color = colorRGBs[(int)mixes[(int)floatToNames[primary], (int)floatToNames[other]]];
		return HexToColor(color);
	}
	
	private static Color HexToColor(uint hex) 
	{
		uint bigint = hex >> 8;
		float r = ((bigint >> 16) & 255) / 256.0f;
		float g = ((bigint >> 8) & 255) / 256.0f;
		float b = (bigint & 255) / 256.0f;
		
		return new Color(r, g, b);
	}
	
	public static Color GetRed()
	{
		return HexToColor(colorRGBs[(int)ColorNames.RED]);
	}

	public static Color GetBlue()
	{
		return HexToColor(colorRGBs[(int)ColorNames.BLUE]);
	}
	
	public static Color GetYellow()
	{
		return HexToColor(colorRGBs[(int)ColorNames.YELLOW]);
	}	
	
//	
//	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
//	private static string ColorToHex(Color32 color)
//	{
//		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
//		return hex;
//	}
//	
//	private static Color StringHexToColor(string hex)
//	{
//		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
//		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
//		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
//		return new Color32(r,g,b, 255);
//	}
}