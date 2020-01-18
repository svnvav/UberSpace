using UnityEngine;

namespace Svnvav.UberSpace
{
    [CreateAssetMenu]
    public class RaceDescription : ScriptableObject
    {
        [SerializeField] private string _name;
        public string Name => _name;
        
        [SerializeField] private string _tagline;
        public string Tagline => _tagline;

        [SerializeField] private bool _isAggressive;
        public bool IsAggressive => _isAggressive;

        [SerializeField] private Sprite _planetSprite;
        [SerializeField] private Sprite _hudInfoSprite;
        [SerializeField] private Sprite _loadingSprite;

        public Sprite PlanetSprite => _planetSprite;
        public Sprite HudInfoSprite => _hudInfoSprite;
        public Sprite LoadingSprite => _loadingSprite;
    }
}