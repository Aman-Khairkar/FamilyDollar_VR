using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour 
{
	private string _upc; //Position.UPC

	public string upc
	{
		get
		{
			return _upc;
		}
		set
		{
			_upc = value;
		}
	}

	private string _id; //Position.ID

	public string id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	private Vector3 _position; //(Position.X, Position.Y, Position.Z) //Z is measured from the back of the planogram

	public Vector3 position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}
	//!!!!THIS MAY ACTUALLY BE WHERE I CALCULATE INGAMESCALE
	//I'm thinking this should not be the Position.Width and all that but rather
	//the product size * facings. Look to inGameScale to see what it should be.
	//this just means we need to GET our product sizing for this to work.
	//size will need more setup than the other values
	private Vector3 _size; //(Position.Width, Position.Height, Position.Depth)

	public Vector3 size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}

	}
	private Vector3 _rotation;//(Position.Slope, Position.Angle, Position.Roll)

	public Vector3 rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
		}
	}

	private Vector3 _facings;//(Position.HFacings, Position.VFacings, Position.DFacings)

	public Vector3 facings
	{
		get
		{
			return _facings;
		}
		set
		{
			_facings = value;
		}
	}

	private Vector3 inGameScale;//(x = size.x * facings.x, y = size.y * facings.y, z = size.z * facings.z);

	public void BuildPosition(float X, float Y, float Z)
	{
		position = new Vector3(X, Y, Z);
	}
}
