using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class JugadorNetworkObject : NetworkObject
	{
		public const int IDENTITY = 6;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _Posicion;
		public event FieldEvent<Vector3> PosicionChanged;
		public InterpolateVector3 PosicionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 Posicion
		{
			get { return _Posicion; }
			set
			{
				// Don't do anything if the value is the same
				if (_Posicion == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_Posicion = value;
				hasDirtyFields = true;
			}
		}

		public void SetPosicionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_Posicion(ulong timestep)
		{
			if (PosicionChanged != null) PosicionChanged(_Posicion, timestep);
			if (fieldAltered != null) fieldAltered("Posicion", _Posicion, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _Rotacion;
		public event FieldEvent<Quaternion> RotacionChanged;
		public InterpolateQuaternion RotacionInterpolation = new InterpolateQuaternion() { LerpT = 0f, Enabled = false };
		public Quaternion Rotacion
		{
			get { return _Rotacion; }
			set
			{
				// Don't do anything if the value is the same
				if (_Rotacion == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_Rotacion = value;
				hasDirtyFields = true;
			}
		}

		public void SetRotacionDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_Rotacion(ulong timestep)
		{
			if (RotacionChanged != null) RotacionChanged(_Rotacion, timestep);
			if (fieldAltered != null) fieldAltered("Rotacion", _Rotacion, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			PosicionInterpolation.current = PosicionInterpolation.target;
			RotacionInterpolation.current = RotacionInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _Posicion);
			UnityObjectMapper.Instance.MapBytes(data, _Rotacion);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_Posicion = UnityObjectMapper.Instance.Map<Vector3>(payload);
			PosicionInterpolation.current = _Posicion;
			PosicionInterpolation.target = _Posicion;
			RunChange_Posicion(timestep);
			_Rotacion = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			RotacionInterpolation.current = _Rotacion;
			RotacionInterpolation.target = _Rotacion;
			RunChange_Rotacion(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Posicion);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _Rotacion);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (PosicionInterpolation.Enabled)
				{
					PosicionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					PosicionInterpolation.Timestep = timestep;
				}
				else
				{
					_Posicion = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_Posicion(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (RotacionInterpolation.Enabled)
				{
					RotacionInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RotacionInterpolation.Timestep = timestep;
				}
				else
				{
					_Rotacion = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_Rotacion(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (PosicionInterpolation.Enabled && !PosicionInterpolation.current.UnityNear(PosicionInterpolation.target, 0.0015f))
			{
				_Posicion = (Vector3)PosicionInterpolation.Interpolate();
				//RunChange_Posicion(PosicionInterpolation.Timestep);
			}
			if (RotacionInterpolation.Enabled && !RotacionInterpolation.current.UnityNear(RotacionInterpolation.target, 0.0015f))
			{
				_Rotacion = (Quaternion)RotacionInterpolation.Interpolate();
				//RunChange_Rotacion(RotacionInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public JugadorNetworkObject() : base() { Initialize(); }
		public JugadorNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public JugadorNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
