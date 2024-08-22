using System.Media;
using SeaFightWinForms.Media.Interfaces;

namespace SeaFightWinForms.Media
{
    class MediaPlayerSound : ISound
    {
        SoundPlayer? seaAndGulls;

        public void Explosion()
        {
            using var explosion = new SoundPlayer(Properties.Resources.ExplosionSound);
            explosion.Play();
        }

        public void Pluk()
        {
            using var pluk = new SoundPlayer(Properties.Resources.PlukSound);
            pluk.Play();
        }

        public void Shot()
        {
            using var shot = new SoundPlayer(Properties.Resources.ShotSound);
            shot.Play();
        }

        public void Rynda()
        {
            using var rynda = new SoundPlayer(Properties.Resources.RyndaSound);
            rynda.Play();
        }

        public void SeaAndGulls()
        {
            using (seaAndGulls = new SoundPlayer(Properties.Resources.SeaAndGullsSound))
            {
                seaAndGulls.Play();
            }
        }

        public void StopSeaAndGulls()
        {
            if (seaAndGulls != null)
            {
                seaAndGulls.Stop();
                seaAndGulls.Dispose();
                seaAndGulls = null;
            }
        }
    }
}
