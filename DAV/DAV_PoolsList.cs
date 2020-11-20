using System;
using System.Linq;
using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.DAV
{
	public class DAV_PoolsList : BasePlugin, IInGameTopPainter, IAfterCollectHandler, INewAreaHandler {
		public float xPos { get; set; }
		public float yPos { get; set; }
		public string PoolSumHeader { get; set; }
		public IFont Font_Pool_Summary { get; set; }
		public Func<int, int, string> PoolSumMessage { get; set; }

		public float mapOffsetX { get; set; }
		public float mapOffsetY { get; set; }
		public IFont Font_Pool { get; set; }
		public IFont Font_Pool_Used { get; set; }
		public Func<int, string> mapPoolMessage { get; set; }

		private uint lastWayPointSno { get; set; } = 0;
		private uint lastWorldId { get; set; } = 0;
		private int[] poolAvailable { get; set; } = new int[] { 0, 0, 0, 0, 0 };
		private List<DAV_WayPointSno> wayPointList { get; set; } = new List<DAV_WayPointSno>();
		private Dictionary<string, DAV_PoolInfo> PoolMarkers { get; set; } = new Dictionary<string, DAV_PoolInfo>();

		public DAV_PoolsList() {
			Enabled = true;
		}

		public void OnNewArea(bool newGame, ISnoArea area) {
			if (newGame) {
				PoolMarkers.Clear();
				lastWorldId = 0;
				lastWayPointSno = 0;
				return;
			}

			if (area.IsTown) {
				lastWorldId = 0;
				lastWayPointSno = 0;
				return;
			}

			if (wayPointList.Any(x => x.AreaSno == area.Sno))
				lastWayPointSno = area.Sno;
		}

		public void AfterCollect() {
			if (Hud.Game.Me.SnoArea == null || Hud.Game.IsInTown || Hud.Game.Me.SnoArea.Code.StartsWith("x1_lr_l")) return;

			var newWayPoint = Hud.Game.Actors.FirstOrDefault(x => x.SnoActor.Sno == ActorSnoEnum._waypoint || x.SnoActor.Sno == ActorSnoEnum._waypoint_oldtristram || x.SnoActor.Sno == ActorSnoEnum._a4_heaven_waypoint);
			if (newWayPoint != null)
				lastWorldId = newWayPoint.WorldId;

			var act = Hud.Game.Me.SnoArea.ActFixed();
			if (act == 0) return; // Cow Level

			foreach (var marker in Hud.Game.Markers.Where(x => x.IsPoolOfReflection)) {
				if (PoolMarkers.ContainsKey(marker.Id)) {
					if (marker.IsUsed != PoolMarkers[marker.Id].IsUsed) {
						PoolMarkers[marker.Id].IsUsed = marker.IsUsed;
						poolAvailable[act - 1]--;
					}
					continue;
				}

				var dist = float.MaxValue;
				var useAreaSno = lastWayPointSno;
				foreach(var waypoint in wayPointList.Where(x => x.WorldId == lastWorldId && x.Act == act)) {
					var thisDist = waypoint.FloorCoordinate.XYDistanceTo(marker.FloorCoordinate);
					if (thisDist < dist) {
						dist = thisDist;
						useAreaSno = waypoint.AreaSno;
					}
				}

				PoolMarkers.Add(marker.Id, new DAV_PoolInfo(marker.IsUsed, act, useAreaSno, marker.Id));
				poolAvailable[act - 1]++;
			}
		}

		public void PaintTopInGame(ClipState clipState) {
			if (PoolMarkers.Count == 0) return;
			if (clipState != ClipState.AfterClip) return;
			if (Hud.Game.SpecialArea != SpecialArea.None) return;

			var SummaryMsg = "";
			for (var i = 0; i < 5; i++) {
				if (poolAvailable[i] > 0)
					SummaryMsg += PoolSumMessage(i + 1, poolAvailable[i]) + "\n";
			}

			if (!string.IsNullOrEmpty(SummaryMsg))
				Font_Pool_Summary.DrawText(PoolSumHeader + "\n" + SummaryMsg, xPos, yPos);

			if (!Hud.Render.WorldMapUiElement.Visible || Hud.Render.ActMapUiElement.Visible) return;

			var mapCurrentAct = Hud.Game.ActMapCurrentAct;
			var w = 110 * Hud.Window.HeightUiRatio;

			var poolList = PoolMarkers.Values.ToList();
			foreach (var waypoint in Hud.Game.ActMapWaypoints.Where(x => x.BountyAct == mapCurrentAct)) {
				var thisList = poolList.Where(x1 => x1.nearWayPointSno == waypoint.TargetSnoArea.Sno);
				var qtyT = thisList.Count();
				if (qtyT == 0) continue;

				var x = Hud.Render.WorldMapUiElement.Rectangle.X + waypoint.CoordinateOnMapUiElement.X * Hud.Window.HeightUiRatio;
				var y = Hud.Render.WorldMapUiElement.Rectangle.Y + waypoint.CoordinateOnMapUiElement.Y * Hud.Window.HeightUiRatio;
				var qty1 = thisList.Count(x2 => x2.IsUsed);
				var qty2 = qtyT - qty1;

				if (qty1 > 0) {
					var layout = Font_Pool_Used.GetTextLayout(mapPoolMessage(qty1));
					Font_Pool_Used.DrawText(layout, x + w - layout.Metrics.Width - mapOffsetX, y + mapOffsetY);
				}
				if (qty2 > 0)
					Font_Pool.DrawText(mapPoolMessage(qty2), x + w + mapOffsetX, y + mapOffsetY);
			}
		}

		public override void Load(IController hud) {
			base.Load(hud);

			xPos = 770; // Hud.Window.Size.Width * 0.4f;
			yPos = 10; // Hud.Window.Size.Height * 0.01f;
			PoolSumHeader = "Pool Quantity Summary";
			PoolSumMessage = (act, qty) => "A" + act.ToString() + " : " + qty.ToString() + " x Pool";
			Font_Pool_Summary = Hud.Render.CreateFont("Arial", 8f, 250, 255, 250, 0, false, false, false);

			mapOffsetX = 6f;
			mapOffsetY = 0f;
			mapPoolMessage = (qty) => "Pool x " + qty.ToString();
			Font_Pool = Hud.Render.CreateFont("Arial", 7f, 255, 255, 255, 51, true, true, 160, 51, 51, 51, true);
			Font_Pool_Used = Hud.Render.CreateFont("Arial", 7f, 120, 255, 255, 51, true, true, 160, 51, 51, 51, true);

			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 91133, 1999831045, 1976.710f, 2788.146f, 41.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19780, 1999962119, 820.000f, 749.307f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19783, 2000027656, 820.000f, 701.500f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19785, 2000093193, 820.000f, 1229.307f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19787, 2000158730, 1059.357f, 578.648f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19954, 1999831045, 2895.395f, 2371.276f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 72712, 1999831045, 2161.802f, 1826.882f, 1.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19952, 1999831045, 2037.839f, 910.656f, 1.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 61573, 1999831045, 1263.054f, 827.765f, 63.1f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19953, 1999831045, 423.106f, 781.156f, 21.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 93632, 1999831045, 2310.500f, 4770.000f, 1.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 78572, 2000617489, 1339.771f, 1159.466f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19941, 1999831045, 1688.828f, 3890.089f, 41.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19943, 1999831045, 1080.022f, 3444.448f, 62.4f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19774, 2000814100, 820.000f, 736.500f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 119870, 2001207322, 56.772f, 146.284f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19775, 2000879637, 452.500f, 820.000f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 1, 19776, 2000945174, 979.000f, 1060.000f, 0.0f));

			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 19836, 2001272859, 2760.302f, 1631.820f, 187.5f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 63666, 2001272859, 1419.449f, 321.863f, 176.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 19835, 2001272859, 1427.769f, 1185.675f, 186.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 460671, 2001403933, 1747.454f, 549.045f, 0.5f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 456638, 2001469470, 400.119f, 889.819f, 0.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 210451, 2001600544, 612.485f, 936.556f, -29.8f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 57425, 2001272859, 3548.838f, 4269.278f, 101.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 62752, 2001797155, 90.000f, 59.500f, 2.7f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 53834, 2001272859, 1257.886f, 3968.300f, 111.8f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 2, 19800, 2002124840, 799.995f, 680.024f, -0.1f));

			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 75436, 1999568897, 1065.148f, 472.449f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 93103, 2002321451, 825.148f, 1192.449f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 136448, 2002386988, 825.148f, 472.449f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 93173, 2002452525, 4272.158f, 4252.097f, -24.8f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 154644, 1999699971, 4356.238f, 408.241f, -2.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 155048, 1999699971, 3452.838f, 609.227f, 0.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 69504, 1999699971, 2708.557f, 586.267f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 86080, 2002583599, 1679.054f, 744.707f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 80791, 2002714673, 1041.245f, 983.039f, -10.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 119305, 2002845747, 2573.769f, 1206.666f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 119653, 2002976821, 959.404f, 1097.913f, -10.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 119306, 2003107895, 1162.255f, 686.218f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 3, 428494, 2003238969, 606.110f, 1065.332f, 0.0f));

			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 109526, 2003435580, 1073.872f, 786.390f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 464857, 2003501117, 1340.086f, 110.277f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 409001, 2003566654, 1339.833f, 359.946f, -1.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 464065, 2003697728, 1030.028f, 870.214f, 15.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 409512, 2003632191, 2072.500f, 2747.500f, -30.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 109514, 2003763265, 873.785f, 1114.032f, -14.9f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 409517, 2003828802, 2520.398f, 1979.950f, 15.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 109538, 2003894339, 1079.837f, 856.173f, -1.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 109540, 2004090950, 345.398f, 359.990f, -1.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 464066, 2004222024, 2500.083f, 1390.092f, 31.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 475856, 2004222024, 348.659f, 341.491f, 50.8f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 475854, 2004287561, 1559.163f, 3636.186f, -23.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 4, 464063, 2004287561, 350.023f, 350.276f, 0.0f));

			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 263493, 2004353098, 1026.961f, 418.969f, 10.8f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 338946, 2004484172, 1260.250f, 540.750f, 3.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 261758, 2004549709, 402.102f, 419.735f, 10.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 283553, 2004680783, 608.812f, 417.468f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 258142, 2004746320, 1140.714f, 540.347f, 12.4f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 283567, 2004877394, 877.314f, 399.194f, 0.0f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 338602, 2004942931, 1433.629f, 220.720f, 0.3f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 271234, 2005139542, 1069.580f, 679.856f, 20.2f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 459863, 2005205079, 1000.500f, 1160.500f, 0.5f));
			wayPointList.Add(new DAV_WayPointSno(Hud, 5, 427763, 2005270616, 661.430f, 423.897f, 2.9f));
		}
	}

	public class DAV_WayPointSno {
		public int Act { get; set; }
		public uint AreaSno { get; set; }
		public uint WorldId { get; set; }
		public IWorldCoordinate FloorCoordinate { get; set; }

		public DAV_WayPointSno(IController hud, int act, uint areaSno, uint worldId, float x, float y, float z) {
			Act = act;
			AreaSno = areaSno;
			WorldId = worldId;
			FloorCoordinate = hud.Window.CreateWorldCoordinate(x, y, z);
		}
	}

	public class DAV_PoolInfo {
		public int Act { get; set; }
		public bool IsUsed { get; set; }
		public uint nearWayPointSno { get; set; }
		public string uniqueID { get; set; }

		public DAV_PoolInfo(bool used, int act, uint areaSno, string id) {
			Act = act;
			IsUsed = used;
			nearWayPointSno = areaSno;
			uniqueID = id;
		}
	}
}