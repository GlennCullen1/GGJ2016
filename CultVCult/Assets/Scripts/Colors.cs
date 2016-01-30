using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Colors
{
	public enum ColorNames
	{
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
		MAX_COLOR,

		WASTE = MAX_COLOR,
		EMPTY
  	};

	private static ColorNames[,] mixes = 	// primary+1 x secondary+1
	{
		{ ColorNames.RED, ColorNames.ORANGE, ColorNames.PURPLE, ColorNames.WASTE, ColorNames.VERMILLION, ColorNames.BYZANTIUM },
		{ ColorNames.ORANGE, ColorNames.YELLOW, ColorNames.GREEN, ColorNames.CHARTREUSE, ColorNames.AMBER, ColorNames.WASTE },
		{ ColorNames.PURPLE, ColorNames.GREEN, ColorNames.BLUE, ColorNames.TURQUOISE, ColorNames.WASTE, ColorNames.PERIWINKLE }
	};

	private static uint[] colorRGBs = 
	{
		0xD63B2BFF,	0xFFEA3DFF,	0x3794EBFF,	0x09921CFF,	0xE9660FFF,	0x6F29EEFF,	
		0x0ECCD1FF,	0xC5F913FF,	0xA73306FF,	0xE88E0EFF,	0xCC2457FF,	0x7F7FF4FF,
		0x3A2B19FF, 0xFFFFFFFF
	};
	
	public static Dictionary<Color, ColorNames> floatToNames = new Dictionary<Color, ColorNames>
	{
		{ HexToColor(0xD63B2BFF), ColorNames.RED }, 	  { HexToColor(0xFFEA3DFF), ColorNames.YELLOW}, 	{ HexToColor(0x3794EBFF), ColorNames.BLUE },
		{ HexToColor(0x09921CFF), ColorNames.GREEN }, 	  { HexToColor(0xE9660FFF), ColorNames.ORANGE}, 	{ HexToColor(0x6F29EEFF), ColorNames.PURPLE },
		{ HexToColor(0x0ECCD1FF), ColorNames.TURQUOISE }, { HexToColor(0xC5F913FF), ColorNames.CHARTREUSE}, { HexToColor(0xA73306FF), ColorNames.VERMILLION },
		{ HexToColor(0xE88E0EFF), ColorNames.AMBER }, 	  { HexToColor(0xCC2457FF), ColorNames.BYZANTIUM}, 	{ HexToColor(0x7F7FF4FF), ColorNames.PERIWINKLE },
		{ HexToColor(0x3A2B19FF), ColorNames.WASTE },     { HexToColor(0xFFFFFFFF), ColorNames.EMPTY}
	};

	/// <summary>
	/// Mixs the colors. 
	/// </summary>
	/// <returns>The colors. If an unacceptable colour is sent in, </returns>
	/// <param name="primary">Primary.</param>
	/// <param name="other">Other.</param>
	public static Color MixColors(Color primary, Color other)
	{
		if (!floatToNames.ContainsKey(primary)  
		    || floatToNames[primary] > ColorNames.PRIMARY 
		    || floatToNames[other] > ColorNames.SECONDARY)
		{
			Debug.LogError(String.Format("ERROR: primary: {0}, other: {1}", primary.ToString(), other.ToString()));
		}
		
		try
		{
			uint color = colorRGBs[(int)mixes[(int)floatToNames[primary], (int)floatToNames[other]]];
			return HexToColor(color);
		}
		catch(Exception)
		{
			Debug.LogError(String.Format("ERROR: primary: {0}, other: {1}", floatToNames[primary].ToString(), floatToNames[other].ToString()));
			throw;
		}
	}
	
	private static Color HexToColor(uint hex) 
	{
		uint bigint = hex >> 8;
		float r = ((bigint >> 16) & 255) / 256.0f;
		float g = ((bigint >> 8) & 255) / 256.0f;
		float b = (bigint & 255) / 256.0f;
		//float a = 1;
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

	public static Color GetRandomColor()
	{
		int num = UnityEngine.Random.Range ((int)0, (int)ColorNames.MAX_COLOR);

		return HexToColor (colorRGBs[num]);
	}

}